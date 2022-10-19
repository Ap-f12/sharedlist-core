namespace SharedListApi.Services
{
    public interface IApiKeyService
    {
        bool IsApiKeyValid(string userId, string apiKey);
        string GenerateApiKey(string userId);
    }
}