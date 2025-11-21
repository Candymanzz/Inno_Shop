using MediatR;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;

namespace UserService.Application.Commands.Users
{
    public record RegisterUserCommand
    (
        string Email,
        string Password,
        string FullName
    ) : IRequest<AuthResponse>;
}
