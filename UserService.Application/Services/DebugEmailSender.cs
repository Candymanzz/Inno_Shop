using UserService.Domain.Interfaces;

namespace UserService.Application.Services
{
    public class DebugEmailSender : IEmailSender
    {
        public Task SendConfirmationEmailAsync(string to, string token)
        {
            Console.WriteLine($"Email to={to}, token={token}");
            return Task.CompletedTask;
        }
    }

}
