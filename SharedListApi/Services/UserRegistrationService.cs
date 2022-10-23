using System.Security.Cryptography;
using System.Text;

namespace SharedListApi.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {

        private static string str => "abcdefghijklmnopqrstuvwyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static string _secretKey = "";

        public UserRegistrationService(IConfiguration configuration)
        {
            _secretKey = configuration["SecretKey"];
        }
        public string GenerateUserId()
        {
            int userIdLength = 16;
            Random random = new Random();
            string userId = "";

            for (int i = 0; i < userIdLength; i++)
            {
                int randomIndex = random.Next(str.Length);
                userId = userId + str[randomIndex];
            }


            return userId;
        }

        public string GenerateToken(string userId)
        {
            var buffer = Encoding.UTF8.GetBytes(userId + _secretKey);
            var bufferHash = SHA256.HashData(buffer);
            var key = BitConverter.ToString(bufferHash).Replace("-", String.Empty);
            return key;

        }
        public bool IsTokenValid(string userId, string apiKey)
        {
            var key = GenerateToken(userId);
            if (apiKey == key)
            {
                return true;
            }
            return false;
        }

    }
}
