using MediatR;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;

namespace UserService.Application.Commands.Auth
{
    public record RefreshTokenCommand 
    (
        string RefreshToken
    ) : IRequest<AuthResponse>;
}
