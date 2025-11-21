using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
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
        [Authorize]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            UserDto? userDto = await mediator.Send(new GetUserByIdQuery(id));
            if (userDto == null)
            {
                return NotFound();
            }

            string? currentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!User.IsInRole("Admin") && currentId != userDto.Id.ToString())
            {
                return Forbid();
            }

            return Ok(userDto);
        }
    }
}
