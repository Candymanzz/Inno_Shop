using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using UserService.Domain.Interfaces;

namespace UserService.Application.Services
{
    public class MailHogEmailSender : IEmailSender
    {
        private readonly SmtpOptions smtpOptions;
        private readonly string frontendUrl;

        public MailHogEmailSender(IOptions<SmtpOptions> smtpOptions, IOptions<FrontendOptions> frontendOptions)
        {
            this.smtpOptions = smtpOptions.Value;
            frontendUrl = frontendOptions.Value.Url;
        }

        public async Task SendConfirmationEmailAsync(string to, string token, Guid id)
        {
            string confirmationLink = $"{frontendUrl}/api/auth/confirm-email?token={token}";
            string body = $"Confirm your account by clicking here: {confirmationLink}  UserId: {id}";

            await SendEmailAsync(to, "Confirm your account", body);
        }

        public async Task SendResetPasswordEmailAsync(string to, string token)
        {
            string resetLink = $"{frontendUrl}/api/auth/reset-password?token={token}";
            string body = $"Your token: {token}";

            await SendEmailAsync(to, "Reset your password", body);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(smtpOptions.Host, smtpOptions.Port)
            {
                EnableSsl = smtpOptions.UseSsl,
                Credentials = new NetworkCredential(smtpOptions.Username, smtpOptions.Password)
            };

            var mailMessage = new MailMessage(smtpOptions.Sender, to, subject, body);
            await client.SendMailAsync(mailMessage);
        }
    }
}
