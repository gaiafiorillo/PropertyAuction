using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using PropertyAuction.Core.Interfaces;

namespace PropertyAuction.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendVerificationEmail(string toEmail, string code)
        {
            await SendEmail(toEmail, "Verify your account", $"Your verification code is: {code}");
        }

        public async Task SendLoginCode(string toEmail, string code)
        {
            await SendEmail(toEmail, "Your login code", $"Your login code is: {code}");
        }

        private async Task SendEmail(string toEmail, string subject, string body)
        {
            var smtp = new SmtpClient(_config["Email:Smtp"])
            {
                Port = int.Parse(_config["Email:Port"]),
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]
                    ),
                EnableSsl = true
            };

            var message = new MailMessage(
                _config["Email:From"],
                toEmail,
                subject,
                body
                );

            await smtp.SendMailAsync( message );
        }
    }
}
