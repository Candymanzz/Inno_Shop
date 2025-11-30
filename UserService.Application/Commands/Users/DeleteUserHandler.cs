using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            await userRepository.DeleteAsync(user);

            return Unit.Value;
        }
    }
}
