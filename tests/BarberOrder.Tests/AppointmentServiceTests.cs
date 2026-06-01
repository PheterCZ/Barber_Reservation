using Xunit;
using Microsoft.EntityFrameworkCore;
using backend.Data;       
using backend.Services;   
using backend.Models;     
using backend.DTOs;      

namespace BarberOrder.Tests; 

public class AppointmentServiceTests
{

    private ApplicationDBContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDBContext(options);
    }

    [Fact]
    public async Task AddBookedSlotAsync_ShouldThrowException_WhenDateIsInPast()
    {
        var context = GetDbContext();
        var service = new AppointmentService(context);
        var customerId = Guid.NewGuid();
        
        var appointmentDto = new AppointmentDto(
            Guid.NewGuid(), customerId, Guid.NewGuid(), "Střih", 
            DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(-0.5), 
            "Jan Novak", "Barber Petr"
        );

        await Assert.ThrowsAsync<ArgumentException>(() => 
            service.AddBookedSlotAsync(appointmentDto, customerId));
    }

    [Fact]
    public async Task AddBookedSlotAsync_ShouldThrowException_WhenDoubleBookingOccurs()
    {
        var context = GetDbContext();
        var service = new AppointmentService(context);
        
        var customerId = Guid.NewGuid();
        context.Users.Add(new ApplicationUser { Id = customerId, FirstName = "Petr", LastName = "Novak", UserName = "Petr Novak"});
        await context.SaveChangesAsync();

        var barberId = Guid.NewGuid();
        var startTime = DateTime.UtcNow.AddDays(1);
        
        var appointmentDto = new AppointmentDto(
            Guid.NewGuid(), customerId, barberId, "Střih", 
            startTime, startTime.AddHours(1), "Jan", "Petr"
        );

        await service.AddBookedSlotAsync(appointmentDto, customerId);

        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            service.AddBookedSlotAsync(appointmentDto, customerId));
    }
}   