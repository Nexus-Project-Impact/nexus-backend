namespace Nexus.Application.Services.Auth
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email, string name, IEnumerable<string> roles);
    }
}