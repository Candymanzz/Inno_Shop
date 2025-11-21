namespace UserService.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; //мне кажется это бизнес
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; } = false;

        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
