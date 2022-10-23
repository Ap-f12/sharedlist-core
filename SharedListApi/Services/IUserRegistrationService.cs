namespace SharedListApi.Services
{
    public interface IUserRegistrationService
    {
        string GenerateToken(string userId);
        string GenerateUserId();
        bool IsTokenValid(string userId, string apiKey);
    }
}