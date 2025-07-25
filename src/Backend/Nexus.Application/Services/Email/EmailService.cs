using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Nexus.Application.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string destinationEmail, string subject, string textMessage, string htmlMessage)
        {
            var emailSettings = _configuration.GetSection("Smtp");
            var smtpServer = emailSettings["Server"];
            var smtpPort = int.Parse(emailSettings["Port"] ?? "587");
            var smtpUser = emailSettings["Username"];
            var smtpPass = emailSettings["Password"];
            var fromEmail = emailSettings["From"] ?? smtpUser;

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(fromEmail));
            message.To.Add(MailboxAddress.Parse(destinationEmail));
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                TextBody = textMessage,
                HtmlBody = htmlMessage
            };
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, false);
            await client.AuthenticateAsync(smtpUser, smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
