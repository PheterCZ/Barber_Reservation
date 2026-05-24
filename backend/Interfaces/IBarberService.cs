using backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public interface IBarberService
    {
        Task<IEnumerable<BarberDto>> GetBarbersAsync();
        Task DeleteBarber(int barberId);
        Task AddBarberAsync(BarberDto barberDto);
    }
}