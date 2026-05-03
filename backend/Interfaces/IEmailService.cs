using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface IEmailService
    {
        public Task SendConfirmationEmailAsync(string email, string subject, string body);
    }
}