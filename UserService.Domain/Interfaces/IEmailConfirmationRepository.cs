using UserService.Domain.Models;

namespace UserService.Domain.Interfaces
{
    public interface IEmailConfirmationRepository
    {
        Task AddAsync(EmailConfirmation confirmation);
        Task<EmailConfirmation?> GetByTokenAsync(string token);
        Task SaveChangesAsync();
    }

}
