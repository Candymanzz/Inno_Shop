using MediatR;
using UserService.Application.DTOs.UserDTOs;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Queries.Users
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, (IEnumerable<UserDto> Item, int Total)>
    {
        private readonly IUserRepository userRepository;

        public GetUsersHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<(IEnumerable<UserDto> Item, int Total)> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            (IEnumerable<User>? items, int total) = await userRepository.GetPagedAsync(request.Page, request.PageSize, request.Q);
            IEnumerable<UserDto> dtoItems = items.Select(u => new UserDto(u.Id, u.Email, u.FullName, u.Role, u.IsActive, u.EmailConfirmed));
            return (dtoItems, total);
        }
    }
}
