using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public record BarberDto(
        Guid Id, 
        
        [Required]
        [MaxLength(50)]
        string FirstName, 
        
        [Required]
        [MaxLength(50)]
        string LastName, 
        
        [Required]
        [Phone]
        string Phone, 
        
        [Required]
        [EmailAddress]
        string Email, 
        
        [MaxLength(200)] 
        string? Specialization, 
        
        [Required]
        DateTime StartWork
    )
    {
        public string FullName => $"{FirstName} {LastName}";
    }
    
}