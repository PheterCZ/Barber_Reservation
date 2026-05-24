using System.Security.Claims;
using backend.DTOs;
using backend.Services;
using BarberOrder.backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BarberController : ControllerBase
    {
        private readonly IBarberService _barberService;
        public BarberController(IBarberService barberService)
        {
            _barberService = barberService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarberDto>>> GetBarbers()
        {
            var barbers = await _barberService.GetBarbersAsync();
            return Ok(barbers);
        }   

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBarber(int id)
        {
            await _barberService.DeleteBarber(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddBarber([FromBody] BarberDto barberDto)
        {
            await _barberService.AddBarberAsync(barberDto);
            return Created();
        }
    }
}