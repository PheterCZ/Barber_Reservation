using backend.Data;
using backend.DTOs;
using backend.Services;
using BarberOrder.backend.Models;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.Phone
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if(result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, UserRoles.User);
        }
        return result;  
    }

    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLower();

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return new AuthResult(false, null, "Uživatel nenalezen");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            await _userManager.AccessFailedAsync(user);
            return new AuthResult(false, null, "Neplatné heslo");
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        var token = await _jwtService.GenerateTokenAsync(user, roles);

        return new AuthResult(true, token, "Úspěšné přihlášení");
    }
}