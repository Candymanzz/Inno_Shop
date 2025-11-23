using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Date;

namespace UserService.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository //!!!доделать
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(User user)
        {
            await appDbContext.Users.AddAsync(user);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            appDbContext.Users.Remove(user);
            await SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<(IEnumerable<User> Items, int Total)> GetPagedAsync(int page, int pageSize, string? q = null) //ref
        {
            IQueryable<User> query = appDbContext.Users;

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(u => u.Email.Contains(q) || u.FullName.Contains(q));
            }

            int total = await query.CountAsync();

            List<User> items = await query
                .OrderBy(u => u.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task SaveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            appDbContext.Users.Update(user);
            await SaveChangesAsync();
        }
    }
}
