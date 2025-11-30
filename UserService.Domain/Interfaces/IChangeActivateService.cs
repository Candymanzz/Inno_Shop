namespace UserService.Domain.Interfaces
{
    public interface IChangeActivateService
    {
        Task ChangeActivateAsync(Guid userId, bool status);
    }
}
