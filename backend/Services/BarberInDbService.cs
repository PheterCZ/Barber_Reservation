using backend.Data;
using backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public static class BarberInDbService
    {
        public static async Task<(ApplicationUser user, string barberName)> BarberDbService(ApplicationDBContext _context, Guid customerId, AppointmentDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == customerId)
                       ?? throw new KeyNotFoundException("Přihlášený uživatel nebyl nalezen.");
            
            var barber = await _context.Barbers
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == dto.BarberId);

            var barberName = barber != null 
                ? $"{barber.FirstName} {barber.LastName}"   
                : dto.BarberName;

            return (user, barberName);
        }
        
    }
}