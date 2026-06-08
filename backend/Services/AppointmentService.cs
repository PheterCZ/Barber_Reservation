using backend.Data;
using backend.DTOs;
using backend.Extensions;
using backend.Interfaces;
using backend.Models;
using backend.Utilities;
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
            return await _context.Appointments
                .Select(a => a.ToDto())
                .ToListAsync();
        }
        public async Task AddBookedSlotAsync(AppointmentDto appointmentDto, Guid customerId)
        {
            var duration = appointmentDto.Service.ParseDurationMinutes();
            var (start, end) = AppointmentValidator.GetValidatedTimes(appointmentDto, duration);

            var (user, barberName) = await BarberInDbService.BarberDbService(_context, customerId, appointmentDto);

            var appointment = await new CreateAppointmentInDbAsync(_context).CreateAsync(customerId, appointmentDto, start, end);

            var emailBody = appointmentDto.GenerateEmailBody(user, barberName);
            try 
            {
                await _emailService.SendConfirmationEmailAsync(user.Email!, "Potvrzení rezervace", emailBody);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Rezervace byla vytvořena, ale e-mail se nepodařilo odeslat na {Email}.", user.Email);
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

    }
}