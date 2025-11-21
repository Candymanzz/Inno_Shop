using MediatR;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;

namespace UserService.Application.Commands.Auth
{
    public class RefreshTokenCommand : IRequest<AuthResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
