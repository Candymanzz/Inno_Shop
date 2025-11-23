namespace UserService.Domain.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string Role { get; }
    }
}
