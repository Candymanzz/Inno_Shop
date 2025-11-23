using MediatR;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;
using UserService.Application.Services;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailConfirmationRepository emailConfirmationRepository;
        private readonly IEmailSender emailSender;
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenService tokenService;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public RegisterUserHandler(IUserRepository userRepository,
            IEmailConfirmationRepository emailConfirmationRepository,
            IEmailSender emailSender,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            this.userRepository = userRepository;
            this.emailConfirmationRepository = emailConfirmationRepository;
            this.emailSender = emailSender;
            this.passwordHasher = passwordHasher;
            this.tokenService = tokenService;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            User? existing = await userRepository.GetByEmailAsync(request.Email);

            if (existing != null)
            {
                throw new Exception("User already exists"); //переделать потом
            }

            string passwordHash = passwordHasher.Hash(request.Password);

            User user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FullName = request.FullName,
                Role = "User"
            };

            await userRepository.AddAsync(user);

            string accessToken = tokenService.GenereateAccessToken(user);
            RefreshToken refreshToken = tokenService.GetRefreshToken(user.Id, TimeSpan.FromDays(10));

            await refreshTokenRepository.AddAsync(refreshToken);
            await refreshTokenRepository.SaveChangesAsync();

            EmailConfirmation confirmation = new EmailConfirmation
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            await emailConfirmationRepository.AddAsync(confirmation);

            await emailSender.SendConfirmationEmailAsync(request.Email, confirmation.Token);

            return new AuthResponse(accessToken, refreshToken.Token);
        }
    }
}
