using backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            _logger.LogInformation("Received registration request for email: {Email}", registerDto.Email);

            var result = await _authService.RegisterAsync(registerDto);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully" });
            }
            var errors = result.Errors.Select(e => e.Description).ToArray();
            _logger.LogError("Registration failed for email: {Email}", registerDto.Email);
            return BadRequest(new { Errors = errors });
        }
        
    }
}

