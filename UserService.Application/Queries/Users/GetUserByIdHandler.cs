using MediatR;
using UserService.Application.DTOs.UserDTOs;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Queries.Users
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserRepository userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.Id);

            if (user == null)
            {
                return null;
            }

            return new UserDto(user.Id, user.Email, user.FullName, user.Role, user.IsActive, user.EmailConfirmed);
        }
    }
}
