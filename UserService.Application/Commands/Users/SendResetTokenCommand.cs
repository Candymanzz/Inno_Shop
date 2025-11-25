using MediatR;

namespace UserService.Application.Commands.Users
{
    public record SendResetTokenCommand(string Email) : IRequest<Unit>;
}
