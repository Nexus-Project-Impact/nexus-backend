using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.Services.Email
{
    public interface IEmailService 
    {
        public Task SendEmailAsync(string destinationEmail, string subject, string textMessage, string htmlMessage);
    }
}
