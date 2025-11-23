using MediatR;

namespace UserService.Application.Commands.Users
{
    public record UpdateUserCommand(
        Guid TargetUserId,
        string? FullName,
        string? Email,
        string? Password,
        string? Role,
        bool? IsActive,
        bool? EmailConfirmed,
        Guid CurrentUserId,
        string CurrentUserRole
    ) : IRequest<Unit>;
}
