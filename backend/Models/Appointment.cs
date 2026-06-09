using System.ComponentModel.DataAnnotations;
using backend.Data;

namespace backend.Models
{
    public class Appointment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Service { get; set; }
        public Guid CustomerId { get; set; }
        public ApplicationUser? Customer { get; set; }

        [Required]
        public Guid BarberId { get; set; }
        public Barber? Barber { get; set; }

    }
}