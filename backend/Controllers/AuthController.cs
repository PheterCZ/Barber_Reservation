using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using BarberOrder.backend.Models;
using Microsoft.AspNetCore.Identity;
using backend.Interfaces;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;
    
        public AuthController(IAuthService authService, ILogger<AuthController> logger, IEmailService emailService)
        {
            _authService = authService;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            _logger.LogInformation("Register attempt: {Email}", dto.Email);

            var result = await _authService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { errors });
            }

            try
            {
                await _emailService.SendConfirmationEmailAsync(dto.Email, "Vítejte u nás!", "Děkujeme vám za registraci!");
                _logger.LogInformation("Welcome email sent to {Email}", dto.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send welcome email to {Email}", dto.Email);
            }

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Error });
            }

            return Ok(new
            {
                token = result.Token
            });
        }
    }
}