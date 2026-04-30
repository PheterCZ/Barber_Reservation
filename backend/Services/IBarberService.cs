

using backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public interface IBarberService
    {
        Task<IEnumerable<string>> GetBarbersAsync();
        Task<BarberDto> CreateBarber(BarberDto barberDto);
        Task DeleteBarber(int barberId);

    }
}