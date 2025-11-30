using MediatR;

namespace UserService.Application.Commands.Users
{
    public record ChangeActivateCommand(Guid UserId, bool Status) : IRequest<Unit>;
}
