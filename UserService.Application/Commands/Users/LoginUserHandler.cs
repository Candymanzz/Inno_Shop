using MediatR;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenService tokenService;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public LoginUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.tokenService = tokenService;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Email not confirmed");
            }

            string accessToken = tokenService.GenereateAccessToken(user);
            RefreshToken refreshToken = tokenService.GetRefreshToken(user.Id, TimeSpan.FromDays(10));

            await refreshTokenRepository.AddAsync(refreshToken);

            return new AuthResponse(accessToken, refreshToken.Token);
        }
    }
}
