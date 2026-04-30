using System.Security.Claims;
using backend.DTOs;
using backend.Services;
using BarberOrder.backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarberController : ControllerBase
    {
        private readonly IBarberService _barberService;
        public BarberController(IBarberService barberService)
        {
            _barberService = barberService;
        }
        [Authorize] // Stačí být jen přihlášený
        [HttpGet("check-my-roles")]
        public IActionResult CheckRoles()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            
            return Ok(new {
                UserName = User.Identity?.Name,
                IsAdmin = User.IsInRole("Admin"), // Tohle je to, co selhává
                AllRolesFoundInClaims = roles,
                RawClaims = claims
            });
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetBarbers()
        {
            var barbers = await _barberService.GetBarbersAsync();
            return Ok(barbers);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<BarberDto>> CreateBarber([FromBody] BarberDto barberDto)
        {
            var createdBarber = await _barberService.CreateBarber(barberDto);
            return CreatedAtAction(nameof(GetBarbers), new { id = createdBarber.Id }, createdBarber);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBarber(int id)
        {
            await _barberService.DeleteBarber(id);
            return NoContent();
        }
    }
}