namespace UserService.Domain.Interfaces
{
    public interface IPasswordHasher //ct
    {
        string Hash(string password);
        bool Verify(string hashedPassword, string providedPassword);
    }
}
