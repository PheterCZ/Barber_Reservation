using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace backend.Data
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)] 
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

    }
}