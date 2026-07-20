namespace backend.Services
{
    public interface IAuthService
    {
        string GenerateToken(string username);
        bool VerifyPassword(string password, string passwordHash);
    }
}
