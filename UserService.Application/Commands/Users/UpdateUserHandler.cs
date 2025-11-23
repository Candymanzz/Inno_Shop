using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly ICurrentUserService currentUserService;

        public UpdateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ICurrentUserService currentUserService)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.TargetUserId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (currentUserService.UserId == user.Id && currentUserService.Role == "User")
            {
                if (request.FullName != null) user.FullName = request.FullName;
                if (request.Email != null) user.Email = request.Email;
                if (request.Password != null) user.PasswordHash = passwordHasher.Hash(request.Password);
            }
            else if (currentUserService.Role == "Admin")
            {
                if (request.FullName != null) user.FullName = request.FullName;
                if (request.Email != null) user.Email = request.Email;
                if (request.Password != null) user.PasswordHash = passwordHasher.Hash(request.Password);
                if (request.Role != null) user.Role = request.Role;
                if (request.IsActive.HasValue) user.IsActive = request.IsActive.Value;
                if (request.EmailConfirmed.HasValue) user.EmailConfirmed = request.EmailConfirmed.Value;
            }
            else
            {
                throw new Exception("Forbidden");
            }

            await userRepository.UpdateAsync(user); 

            return Unit.Value;
        }
    }
}
