using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Barber
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;


        public string Phone { get; set; } = string.Empty;


        public string Email {get; set; } = string.Empty;


        public string? Specialization { get; set; }
        
        public DateTime StartWork { get; set; }
    }
}