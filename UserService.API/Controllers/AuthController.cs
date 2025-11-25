using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands.Auth;
using UserService.Application.Commands.Users;
using UserService.Application.DTOs.AuthDTOs.RequestDTOs;
using UserService.Application.DTOs.AuthDTOs.ResponseDTOs;
using UserService.Application.DTOs.UserDTOs;
using UserService.Application.Services;
using UserService.Domain.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
        {
            AuthResponse result = await mediator.Send(new RegisterUserCommand(
                registerRequest.Email,
                registerRequest.Password,
                registerRequest.FullName
                ));
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {
            AuthResponse result = await mediator.Send(new LoginUserCommand(
                loginRequest.Email,
                loginRequest.Password
                ));

            Response.Cookies.Append("a-token-cookies", result.AccessToken);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshRequest refreshRequest)
        {
            AuthResponse result = await mediator.Send(new RefreshTokenCommand(refreshRequest.RefreshToken));
            return Ok(result);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var result = await mediator.Send(new ConfirmEmailCommand(token));
            return Ok(new { message = result });
        }

        [HttpPost("send-reset-token")]
        public async Task<IActionResult> SendResetToken([FromBody] SendResetTokenRequest request)
        {
            await mediator.Send(new SendResetTokenCommand(request.Email));
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await mediator.Send(new ResetPasswordCommand(request.Token, request.NewPassword));
            return Ok();
        }
    }
}
