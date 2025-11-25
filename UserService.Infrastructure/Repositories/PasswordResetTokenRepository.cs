using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Date;

namespace UserService.Infrastructure.Repositories
{
    internal class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly AppDbContext appDbContext;
        public PasswordResetTokenRepository(AppDbContext appDbContext) 
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(PasswordResetToken token)
        {
            await appDbContext.PasswordResetTokens.AddAsync(token);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<PasswordResetToken?> GetByTokenAsync(string token)
        {
            return await appDbContext.PasswordResetTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task SaveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(PasswordResetToken token)
        {
            appDbContext.PasswordResetTokens.Remove(token);
            await SaveChangesAsync();
        }
    }

}
