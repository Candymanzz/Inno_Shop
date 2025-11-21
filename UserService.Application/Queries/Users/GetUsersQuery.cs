using MediatR;
using UserService.Application.DTOs.UserDTOs;

namespace UserService.Application.Queries.Users
{
    public record GetUsersQuery
    (
        int Page = 1,
        int PageSize = 20,
        string? Q = null
    ) : IRequest<(IEnumerable<UserDto> Items, int Total)>;
}
