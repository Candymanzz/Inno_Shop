using UserService.Domain.Models;

namespace UserService.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenereateAccessToken(User user);
        RefreshToken GetRefreshToken(Guid userId, TimeSpan ttl);
    }
}
