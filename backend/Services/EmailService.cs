
using backend.Interfaces;
using BarberOrder.backend.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
        }

        public async Task SendConfirmationEmailAsync(string email, string subject, string body)
        {     
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.Value.FromAddress, _emailSettings.Value.FromAddress));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_emailSettings.Value.SmtpServer, _emailSettings.Value.Port, _emailSettings.Value.EnableSsl);
                await client.AuthenticateAsync(_emailSettings.Value.Username, _emailSettings.Value.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", email);
                throw;
            }
        }  
    }
}