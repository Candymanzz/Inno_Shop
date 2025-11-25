using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class SendResetTokenHandler : IRequestHandler<SendResetTokenCommand, Unit>
    {
        private readonly IUserRepository userRepo;
        private readonly IPasswordResetTokenRepository tokenRepo;
        private readonly IEmailSender emailSender;

        public SendResetTokenHandler(IUserRepository userRepo, IPasswordResetTokenRepository tokenRepo, IEmailSender emailSender)
        {
            this.userRepo = userRepo;
            this.tokenRepo = tokenRepo;
            this.emailSender = emailSender;
        }

        public async Task<Unit> Handle(SendResetTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepo.GetByEmailAsync(request.Email);
            if (user == null) return Unit.Value;

            string token = Guid.NewGuid().ToString();

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            await tokenRepo.AddAsync(resetToken);
            await emailSender.SendResetPasswordEmailAsync(request.Email, token);

            return Unit.Value;
        }
    }
}
