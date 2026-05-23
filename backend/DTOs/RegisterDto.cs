using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public record RegisterDto(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Phone
    );
}





