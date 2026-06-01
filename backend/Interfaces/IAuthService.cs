using backend.DTOs;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<AuthResult> LoginAsync(LoginDto dto);
    }
}
