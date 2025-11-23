using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Application.Commands.Users;
using UserService.Application.DTOs.UserDTOs;
using UserService.Application.Queries.Users;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null)
        {
            (IEnumerable<UserDto>? items, int total) = await mediator.Send(new GetUsersQuery(page, pageSize, q));
            return Ok(new { items, total, page, pageSize });
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            UserDto? userDto = await mediator.Send(new GetUserByIdQuery(id));
            if (userDto == null)
            {
                return NotFound();
            }

            string? currentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && currentId != userDto.Id.ToString())
            {
                return Forbid();
            }

            return Ok(userDto);
        }

        [HttpPatch("{id:guid}/set-admin")]
        [Authorize]
        public async Task<IActionResult> SetAdminRoleAsync(Guid id)
        {
            await mediator.Send(new SetAdminRoleCommand(id));
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await mediator.Send(new DeleteUserCommand(id));
            return Ok();
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            UserDto? userDto = await mediator.Send(new GetUserByEmailQuery(email));
            return Ok(userDto);
        }

        [HttpPut("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            string currentUserId = User.FindFirst("userId")!.Value;
            string currentUserRole = User.FindFirst(ClaimTypes.Role)!.Value;

            UpdateUserCommand updateUserCommand = new UpdateUserCommand(
            TargetUserId: id,
            FullName: updateUserDto.FullName,
            Email: updateUserDto.Email,
            Password: updateUserDto.Password,
            Role: updateUserDto.Role,
            IsActive: updateUserDto.IsActive,
            EmailConfirmed: updateUserDto.EmailConfirmed,
            CurrentUserId: Guid.Parse(currentUserId),
            CurrentUserRole: currentUserRole ?? "User"
            );

            var updatedUser = await mediator.Send(updateUserCommand);

            return Ok();
        }

        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeActivate(Guid id, [FromBody] bool activity)
        {
            throw new Exception();
        }
    }
}
