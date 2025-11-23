namespace UserService.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task SendConfirmationEmailAsync(string to, string token);
    }
}
