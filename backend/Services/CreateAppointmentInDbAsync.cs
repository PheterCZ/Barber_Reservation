using System.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class CreateAppointmentInDbAsync
    {
        private readonly ApplicationDBContext _context;

        public CreateAppointmentInDbAsync(ApplicationDBContext context) => _context = context;

        public async Task<Appointment> CreateAsync(Guid customerId, AppointmentDto dto, DateTime start, DateTime end)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                var hasOverlap = await _context.Appointments.AnyAsync(a =>
                    a.BarberId == dto.BarberId && a.StartTime < end && a.EndTime > start);

                if (hasOverlap) throw new InvalidOperationException("Termín je již obsazen.");

                var appointment = new Appointment {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    BarberId = dto.BarberId,
                    Service = dto.Service,
                    StartTime = start,
                    EndTime = end
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return appointment;
            }
            catch { await transaction.RollbackAsync(); throw; }
        }
    }
}