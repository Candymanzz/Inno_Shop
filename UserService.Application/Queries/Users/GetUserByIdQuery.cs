using MediatR;
using UserService.Application.DTOs.UserDTOs;

namespace UserService.Application.Queries.Users
{
    public record GetUserByIdQuery
    (
        Guid Id
    ) : IRequest<UserDto?>;
}
