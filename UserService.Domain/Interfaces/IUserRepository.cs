using UserService.Domain.Models;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChangesAsync();
        Task<(IEnumerable<User> Items, int Total)> GetPagedAsync(int page, int pageSize, string? q = null);
        Task ChangeActivateAsync(User user);
    }
}
