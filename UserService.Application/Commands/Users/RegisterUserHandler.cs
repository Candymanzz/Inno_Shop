using MediatR;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenService tokenService;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        //private readonly IEmailConfirmationRepository emailConfirmationRepository;
        //private readonly 

        public RegisterUserHandler(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.tokenService = tokenService;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            User? existing = await userRepository.GetByEmailAsync(request.Email);

            if (existing != null)
            {
                throw new Exception("User already exists");
            }

            string passwordHash = passwordHasher.Hash(request.Password);

            User user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FullName = request.FullName
            };

            await userRepository.AddAsync(user);

            string accessToken = tokenService.GenereateAccessToken(user);
            RefreshToken refreshToken = tokenService.GetRefreshToken(user.Id, TimeSpan.FromDays(10));

            await refreshTokenRepository.AddAsync(refreshToken);
            await refreshTokenRepository.SaveChangesAsync();

            return new AuthResponse(accessToken, refreshToken.Token);
        }
    }
}
