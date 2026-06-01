using backend.DTOs;

namespace backend.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
    }
}