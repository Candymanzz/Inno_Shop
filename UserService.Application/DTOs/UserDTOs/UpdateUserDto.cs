namespace UserService.Application.DTOs.UserDTOs
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } 
        public bool? IsActive { get; set; } 
        public bool? EmailConfirmed { get; set; } 
    }
}
