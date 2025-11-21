using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
        }

        public string GenereateAccessToken(User user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())];

            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(jwtOptions.ExpiresHours));

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            
            return tokenValue;
        }

        public RefreshToken GetRefreshToken(Guid userId, TimeSpan ttl)
        {
            throw new NotImplementedException();
        }
    }
}
