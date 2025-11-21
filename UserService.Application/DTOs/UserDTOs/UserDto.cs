namespace UserService.Application.DTOs.UserDTOs
{
    public record UserDto
    (
        Guid Id,
        string Email,
        string FullName,
        string Role,
        bool IsActive,
        bool EmailConfirmed
    );
}
