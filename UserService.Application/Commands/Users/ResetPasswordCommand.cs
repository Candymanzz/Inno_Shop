using MediatR;

namespace UserService.Application.Commands.Users
{
    public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Unit>;
}
