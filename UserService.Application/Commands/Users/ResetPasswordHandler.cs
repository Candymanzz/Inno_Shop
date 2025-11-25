using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IUserRepository userRepo;
        private readonly IPasswordResetTokenRepository tokenRepo;
        private readonly IPasswordHasher hasher;

        public ResetPasswordHandler(IUserRepository userRepo, IPasswordResetTokenRepository tokenRepo, IPasswordHasher hasher)
        {
            this.userRepo = userRepo;
            this.tokenRepo = tokenRepo;
            this.hasher = hasher;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var resetToken = await tokenRepo.GetByTokenAsync(request.Token);

            if (resetToken == null || resetToken.Used || resetToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid token");

            var user = await userRepo.GetByIdAsync(resetToken.UserId)
                ?? throw new Exception("User not found");

            user.PasswordHash = hasher.Hash(request.NewPassword);
            resetToken.Used = true;

            await userRepo.SaveChangesAsync();
            await tokenRepo.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
