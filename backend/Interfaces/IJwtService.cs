using System.Security.Claims;
using backend.Data;

namespace backend.Services
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user, IList<string> roles);
        Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
    }
}

