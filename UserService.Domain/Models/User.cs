namespace UserService.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true; //мне кажется это бизнес
        public bool EmailConfirmed { get; set; } = false;

        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public EmailConfirmation EmailConfirmation { get; set; } = new EmailConfirmation();
        public List<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    }
}
