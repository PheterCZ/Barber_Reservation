
namespace backend.DTOs
{
    public record AppointmentDto(
        Guid Id, 
        Guid CustomerId, 
        Guid BarberId,   
        string Service, 
        DateTime StartTime, 
        DateTime EndTime,

        string CustomerName, 
        string BarberName
    );
}