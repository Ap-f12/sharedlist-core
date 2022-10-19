using System.Security.Cryptography;
using System.Text;

namespace SharedListApi.Services
{
    public class ApiKeyService : IApiKeyService
    {

        private string _secretKey;

        public ApiKeyService(IConfiguration configuration)
        {
            _secretKey = configuration["SecretKey"];
        }

        public string GenerateApiKey(string userId)
        {
            var buffer = Encoding.UTF8.GetBytes(userId + _secretKey);
            var bufferHash = SHA256.HashData(buffer);
            var key = BitConverter.ToString(bufferHash).Replace("-", String.Empty);
            return key;

        }
        public bool IsApiKeyValid(string userId, string apiKey)
        {
            var key = GenerateApiKey(userId);
            if(apiKey == key)
            {
                return true;
            }
            return false;
        }
    }
}
