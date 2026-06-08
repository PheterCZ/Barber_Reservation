using backend.DTOs;
using backend.Models;

namespace backend.Extensions
{
    public static class AppointmentExtension
    {
        public static AppointmentDto ToDto(this Appointment a)
        {
            return new AppointmentDto(
                a.Id,                                                                     
                a.CustomerId,                                                            
                a.BarberId,                                                               
                a.Service,                                                                
                a.StartTime,                                                              
                a.EndTime,                                                                
                a.Customer != null ? $"{a.Customer.FirstName} {a.Customer.LastName}" : "Neznámý zákazník", 
                a.Barber != null ? a.Barber.FirstName + " " + a.Barber.LastName : "Neznámý barber"    
            );
        }

    }
}