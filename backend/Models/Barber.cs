using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Barber
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; } 


        public string Phone { get; set; }


        public string Email {get; set; }


        public string? Specialization { get; set; }
        
        public DateTime StartWork { get; set; }
    }
}