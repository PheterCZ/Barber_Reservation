using backend.DTOs;

namespace backend.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetBookedSlotsAsync();
        Task AddBookedSlotAsync(AppointmentDto appointmentDto, Guid customerId);
    }
}