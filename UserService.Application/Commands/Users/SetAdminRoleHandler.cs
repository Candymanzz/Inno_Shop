using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class SetAdminRoleHandler : IRequestHandler<SetAdminRoleCommand, Unit>
    {
        private readonly IUserRepository userRepository;

        public SetAdminRoleHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(SetAdminRoleCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Role = "Admin";

            await userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
