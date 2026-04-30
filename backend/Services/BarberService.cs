
using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.DTOs;
using backend.Models;
using BarberOrder.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class BarberService : IBarberService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        public BarberService(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> GetBarbersAsync()
        {
            var barbers = await _context.Barbers
            .ProjectTo<BarberDto>(_mapper.ConfigurationProvider)
            .Select(b => b.FullName)
            .ToListAsync();


            return barbers;
        }   

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<BarberDto> CreateBarber(BarberDto barberDto)
        {
            var barber = _mapper.Map<Barber>(barberDto);

            _context.Barbers.Add(barber);
            await _context.SaveChangesAsync();

            return _mapper.Map<BarberDto>(barber);
             
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
    }
}