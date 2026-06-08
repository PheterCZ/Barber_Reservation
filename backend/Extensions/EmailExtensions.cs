using backend.Data;
using backend.DTOs;

namespace backend.Extensions
{
    public static class EmailExtensions
    {
        public static string GenerateEmailBody(this AppointmentDto dto,ApplicationUser user ,string barberName)
        {
            var fullName = !string.IsNullOrWhiteSpace(user.FirstName) || !string.IsNullOrWhiteSpace(user.LastName)
                ? $"{user.FirstName} {user.LastName}"
                : user.Email;

            var localStart = dto.StartTime.ToLocalTime();
            return $@"Dobrý den {fullName}" +
                   $"Vaše rezervace byla úspěšně vytvořena." +
                   $"Barber: {barberName}" +
                   $"Služba: {dto.Service}" +
                   $"Datum: {localStart:dd.MM.yyyy}" +
                   $"Čas: {localStart:HH:mm}" +
                   "Děkujeme za rezervaci.";
        }
    }
}

