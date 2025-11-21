using MediatR;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;

namespace UserService.Application.Commands.Users
{
    public record LoginUserCommand
    (
        string Email,
        string Password
    ) : IRequest<AuthResponse>;
}
