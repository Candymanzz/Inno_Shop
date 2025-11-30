using Microsoft.Extensions.Options;
using UserService.Domain.Interfaces;

namespace UserService.Application.Services
{
    public class DebugEmailSender : IEmailSender
    {
        private readonly string frontendUrl;

        public DebugEmailSender(IOptions<FrontendOptions> options) //http://localhost:5248
        {
            frontendUrl = options.Value.Url;
        }

        public Task SendConfirmationEmailAsync(string to, string token, Guid id)
        {
            string confirmationLink = $"{frontendUrl}/api/auth/confirm-email?token={token}";

            Console.WriteLine($"[DEBUG] Email to={to}");
            Console.WriteLine($"[DEBUG] User id={id}");
            Console.WriteLine($"[DEBUG] Confirm your account: {confirmationLink}");

            return Task.CompletedTask;
        }

        public Task SendResetPasswordEmailAsync(string to, string token)
        {
            string resetLink = $"{frontendUrl}/api/auth/reset-password?token={token}";

            Console.WriteLine($"[DEBUG] Email to={to}");
            Console.WriteLine($"[DEBUG] Reset your password: {resetLink}");

            return Task.CompletedTask;
        }
    }
}
