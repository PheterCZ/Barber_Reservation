using backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public interface IBarberService
    {
        Task<IEnumerable<string>> GetBarbersAsync();
        Task DeleteBarber(int barberId);

    }
}