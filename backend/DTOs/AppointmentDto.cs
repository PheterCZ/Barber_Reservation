using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Models;

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