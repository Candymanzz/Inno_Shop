using MediatR;

namespace UserService.Application.Commands.Users
{
    public record SetAdminRoleCommand(
        Guid UserId
        ) : IRequest<Unit>;
}
