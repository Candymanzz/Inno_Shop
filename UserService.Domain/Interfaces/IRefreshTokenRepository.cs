using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task UpdateAsync(RefreshToken token);
        Task<IEnumerable<RefreshToken>> GetRefreshTokenByUserIdAsync(Guid userId);
        Task DeleteAsync(RefreshToken refreshToken);
        Task SaveChangesAsync();
    }
}
