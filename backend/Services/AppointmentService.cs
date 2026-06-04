using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace backend.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDBContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            ApplicationDBContext context,
            IEmailService emailService,
            ILogger<AppointmentService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IEnumerable<AppointmentDto>> GetBookedSlotsAsync()
        {
            var appointments = await _context.Appointments
            .Select(a => new AppointmentDto(
                a.Id,                                                                     
                a.CustomerId,                                                            
                a.BarberId,                                                               
                a.Service,                                                                
                a.StartTime,                                                              
                a.EndTime,                                                                
                a.Customer != null ? $"{a.Customer.FirstName} {a.Customer.LastName}" : "Neznámý zákazník", 
                a.Barber != null ? a.Barber.FirstName + " " + a.Barber.LastName : "Neznámý barber"                       
            ))
            .ToListAsync();
            return appointments;
        }


        public async Task AddBookedSlotAsync(AppointmentDto appointmentDto, Guid customerId)
        {
            var startUtc = appointmentDto.StartTime.ToUniversalTime();
            var durationMinutes = ParseDurationMinutes(appointmentDto.Service);
            var endUtc = startUtc.AddMinutes(durationMinutes);

            if(startUtc < DateTime.UtcNow)
            {
                throw new ArgumentException("Čas začátku musí být v budoucnosti.");
            }

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == customerId);
            if (userInDb == null)
            {
                throw new KeyNotFoundException("Přihlášený uživatel nebyl nalezen.");
            }
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                var hasOverlap = await _context.Appointments.AnyAsync(a =>
                    a.BarberId == appointmentDto.BarberId &&
                    a.StartTime < endUtc &&
                    a.EndTime > startUtc);

                if (hasOverlap)
                {
                    throw new InvalidOperationException("Termín je již obsazen nebo se překrývá s jinou rezervací.");
                }

                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    BarberId = appointmentDto.BarberId,
                    Service = appointmentDto.Service,
                    StartTime = startUtc,
                    EndTime = endUtc,
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch(DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Chyba při ukládání rezervace. Zkontrolujte, zda barber není již zaneprázdněn v tomto čase.", ex);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            var barber = await _context.Barbers
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == appointmentDto.BarberId);

            var barberName = barber != null
                ? $"{barber.FirstName} {barber.LastName}"
                : appointmentDto.BarberName;

            var localStart = appointmentDto.StartTime.ToLocalTime();
            var emailSubject = "Potvrzení rezervace";
            var emailBody = $"Dobrý den {userInDb.FirstName},\n\n" +
                            $"Vaše rezervace byla úspěšně vytvořena.\n" +
                            $"Barber: {barberName}\n" +
                            $"Služba: {appointmentDto.Service}\n" +
                            $"Datum: {localStart:dd.MM.yyyy}\n" +
                            $"Čas: {localStart:HH:mm}\n\n" +
                            "Děkujeme za rezervaci.";

            try
            {
                await _emailService.SendConfirmationEmailAsync(userInDb.Email!, emailSubject, emailBody);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Rezervace byla vytvořena, ale potvrzovací e-mail se nepodařilo odeslat na {Email}.", userInDb.Email);
            }
        }

        public async Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(Guid barberId, DateTime date)
        {
            var startOfDay = date.Date;
            var now = DateTime.UtcNow;

            var bookedHours = await _context.Appointments
                .Where(a => a.BarberId == barberId && a.StartTime.Date == startOfDay)
                .Select(a => a.StartTime.Hour)
                .ToListAsync();

            var bookedSet = new HashSet<int>(bookedHours);
            var availableSlots = new List<DateTime>();

            for (int hour = 8; hour < 20; hour++)
            {
                var slotTime = startOfDay.AddHours(hour);

                if (!bookedSet.Contains(hour) && slotTime > now)
                {
                    availableSlots.Add(slotTime);
                }
            }

            return availableSlots;
        }

        private static int ParseDurationMinutes(string serviceLabel)
        {
            var match = Regex.Match(serviceLabel ?? string.Empty, @"\((\d+)\s*min\)", RegexOptions.IgnoreCase);
            if (match.Success && int.TryParse(match.Groups[1].Value, out var parsedMinutes))
            {
                return Math.Clamp(parsedMinutes, 15, 240);
            }

            return 60;
        }
    }
}