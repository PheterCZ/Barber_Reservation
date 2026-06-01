using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDBContext _context;
        public AppointmentService(ApplicationDBContext context)
        {
            _context = context;
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
            var endUtc = appointmentDto.EndTime.ToUniversalTime();

            if(startUtc < DateTime.UtcNow)
            {
                throw new ArgumentException("Čas začátku musí být v budoucnosti.");
            }

            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == customerId);
            if (userInDb == null)
            {
                throw new KeyNotFoundException("Přihlášený uživatel nebyl nalezen.");
            }
            var exists = await _context.Appointments.AnyAsync(a => 
                a.BarberId == appointmentDto.BarberId && 
                a.StartTime == appointmentDto.StartTime);

            if (exists)
            {
                throw new InvalidOperationException("Termín je již obsazen.");
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
            try{
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                throw new InvalidOperationException("Chyba při ukládání rezervace. Zkontrolujte, zda barber není již zaneprázdněn v tomto čase.", ex);
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

            for (int hour = 8; hour < 18; hour++)
            {
                var slotTime = startOfDay.AddHours(hour);

                if (!bookedSet.Contains(hour) && slotTime > now)
                {
                    availableSlots.Add(slotTime);
                }
            }

            return availableSlots;
        }
    }
}