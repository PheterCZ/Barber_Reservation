using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public record RegisterDto(
        [Required, EmailAddress] string Email,
        [Required, MinLength(6)] string Password,
        [Required] string FirstName,
        [Required] string LastName,
        string Phone
    );
}





