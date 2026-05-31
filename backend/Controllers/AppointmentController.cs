
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using backend.DTOs;
using backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetBookedSlots()
        {
            var appointments = await _appointmentService.GetBookedSlotsAsync();
            return Ok(appointments);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddBookedSlotAsync([FromBody] AppointmentDto appointmentDto)
        {
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(customerIdClaim, out var customerId))
            {
                return Unauthorized();
            }

            await _appointmentService.AddBookedSlotAsync(appointmentDto, customerId);
            return Ok();
        }
    }
}