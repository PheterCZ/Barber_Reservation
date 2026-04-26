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
        [Required] [MaxLength(50)] string FirstName,
        [Required] [MaxLength(50)] string LastName,
        [MaxLength(20)] string Phone
    );
}





