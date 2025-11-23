using MediatR;

namespace UserService.Application.Commands.Users
{
    public record DeleteUserCommand(
        Guid UserId
        ) : IRequest<Unit>;
}
