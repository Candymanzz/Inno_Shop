using UserService.Domain.Models;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<(IEnumerable<User> Items, int Total)> GetPagedAsync(int page, int pageSize, string? q = null);
    }
}
