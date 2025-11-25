using UserService.Domain.Models;

namespace UserService.Domain.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetByTokenAsync(string token);
        Task SaveChangesAsync();
        Task DeleteAsync(PasswordResetToken token);
    }
}
