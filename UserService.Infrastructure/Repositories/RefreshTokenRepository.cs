using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Date;

namespace UserService.Infrastructure.Repositories
{
    internal class RefreshTokenRepository : IRefreshTokenRepository //!!!доделать
    {
        private readonly AppDbContext appDbContext;
        public RefreshTokenRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await appDbContext.RefreshTokens.AddAsync(refreshToken);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(RefreshToken refreshToken)
        {
            appDbContext.RefreshTokens.Remove(refreshToken);
            await SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await appDbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task<IEnumerable<RefreshToken>> GetRefreshTokenByUserIdAsync(Guid userId)
        {
            return await appDbContext.RefreshTokens.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            appDbContext.RefreshTokens.Update(token);
            await SaveChangesAsync();
        }
    }
}
