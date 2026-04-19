using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public record BarberDto(Guid Id, string FirstName, string LastName, string Phone, string Email, string? Specialization, DateTime StartWork)
    {
        public string FullName => $"{FirstName} {LastName}";
    }
}