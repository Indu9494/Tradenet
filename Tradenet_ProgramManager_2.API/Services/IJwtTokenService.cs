namespace Tradenet_ProgramManager_2.API.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string email, List<string> roles);
    }
}
