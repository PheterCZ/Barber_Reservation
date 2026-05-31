
namespace backend.Interfaces
{
    public interface IEmailService
    {
        public Task SendConfirmationEmailAsync(string email, string subject, string body);
    }
}