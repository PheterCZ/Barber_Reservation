using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;

namespace backend.Models
{
    public class Appointment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string Service { get; set; } = string.Empty;
        [Required]
        public Guid CustomerId { get; set; }
        public ApplicationUser? Customer { get; set; }

        [Required]
        public Guid BarberId { get; set; }
        public Barber? Barber { get; set; }

    }
}