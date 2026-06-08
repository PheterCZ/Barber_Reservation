using backend.DTOs;

namespace backend.Utilities
{
    public static class AppointmentValidator
    {
        public static (DateTime Start, DateTime End) GetValidatedTimes(AppointmentDto dto, int duration)
        {
            var start = dto.StartTime.ToUniversalTime();

            if (start <= DateTime.UtcNow)
            {
                throw new ArgumentException("Čas začátku musí být v budoucnosti.");
            }

            var end = start.AddMinutes(duration);

            return (start, end);
        }
    }
}