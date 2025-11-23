using MediatR;
using UserService.Application.DTOs.UserDTOs;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Queries.Users
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserDto?>
    {
        private readonly IUserRepository userRepository;

        public GetUserByEmailHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);
            
            if (user == null)
            {
                return null;
            }

            return new UserDto(user.Id, user.Email, user.FullName, user.Role, user.IsActive, user.EmailConfirmed);
        }
    }
}
