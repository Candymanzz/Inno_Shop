using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        private readonly IEmailConfirmationRepository emailConfirmationRepository;
        private readonly IUserRepository userRepository;

        public ConfirmEmailHandler(
            IEmailConfirmationRepository emailConfirmationRepository,
            IUserRepository userRepository)
        {
            this.emailConfirmationRepository = emailConfirmationRepository;
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken ct)
        {
            EmailConfirmation? confirmation = await emailConfirmationRepository.GetByTokenAsync(request.Token);

            if (confirmation == null || confirmation.IsUsed || confirmation.ExpiresAt < DateTime.UtcNow)
            { 
                throw new Exception("Invalid or expired confirmation token"); 
            }

            var user = await userRepository.GetByIdAsync(confirmation.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.EmailConfirmed = true;
            confirmation.IsUsed = true;

            await userRepository.SaveChangesAsync();
            await emailConfirmationRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
