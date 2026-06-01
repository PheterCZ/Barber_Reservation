using backend.DTOs;

namespace backend.Services
{
    public interface IBarberService
    {
        Task<IEnumerable<BarberDto>> GetBarbersAsync();
        Task DeleteBarber(Guid barberId);
        Task AddBarberAsync(BarberDto barberDto);
    }
}