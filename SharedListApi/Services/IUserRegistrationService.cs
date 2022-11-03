using SharedListModels;

namespace SharedListApi.Services
{
    public interface IUserRegistrationService
    {
        UserCredentialsModel RegisterUser();
        bool IsTokenValid(string userId, string apiKey);
    }
}