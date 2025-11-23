using MediatR;

namespace UserService.Application.Commands.Users
{
    public record ConfirmEmailCommand(string Token) : IRequest<Unit>;
}
