
using backend.DTOs;
using Microsoft.EntityFrameworkCore;
namespace backend.Services
{
    public class BarberService : IBarberService
    {
        private readonly ApplicationDBContext _context;

        public BarberService(ApplicationDBContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<BarberDto>> GetBarbersAsync()
        {
            var barbers = await _context.Barbers
                .Select(b => new BarberDto(
                    b.Id,
                    b.FirstName,
                    b.LastName,
                    b.Phone,
                    b.Email,
                    b.Specialization,
                    b.StartWork
                ))
                .ToListAsync();

            return barbers; 
        }
        public async Task DeleteBarber(int barberId)
        {
            var barber = await _context.Barbers.FindAsync(barberId);
            if (barber == null)
            {
                throw new KeyNotFoundException("Barber not found");
            }
            _context.Barbers.Remove(barber);
            await _context.SaveChangesAsync();
        }

        public async Task AddBarberAsync(BarberDto barberDto)
        {
            var barber = new Models.Barber
            {
                FirstName = barberDto.FirstName,
                LastName = barberDto.LastName,
                Email = barberDto.Email,
                Phone = barberDto.Phone,
                Specialization = barberDto.Specialization,
                StartWork = barberDto.StartWork
                
            };

            _context.Barbers.Add(barber);
            await _context.SaveChangesAsync();
        }
    }
}