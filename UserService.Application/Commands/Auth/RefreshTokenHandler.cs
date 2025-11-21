using MediatR;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Auth
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;

        public RefreshTokenHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token");
            }

            User? user = await userRepository.GetByIdAsync(refreshToken.UserId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            string newAccessToken = tokenService.GenereateAccessToken(user);
            RefreshToken newRefreshToken = tokenService.GetRefreshToken(user.Id, TimeSpan.FromDays(10));

            await refreshTokenRepository.AddAsync(newRefreshToken);
            await refreshTokenRepository.DeleteAsync(refreshToken);
            await refreshTokenRepository.SaveChangesAsync();

            return new AuthResponse(newAccessToken, newRefreshToken.Token);
        }
    }
}
