using MediatR;
using UserService.Application.DTOs.UserDTOs;

namespace UserService.Application.Queries.Users
{
    public record GetUserByEmailQuery(
        string Email
        ) : IRequest<UserDto?>;
}
