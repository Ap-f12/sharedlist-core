using Microsoft.AspNetCore.DataProtection.KeyManagement;
using SharedListModels;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Text.Json;

namespace SharedListApi.Services
{
    public class ShareCheckListService : IShareCheckListService
    {
        private string _secretKey;
        

        public ShareCheckListService(IConfiguration configuration)
        {
            _secretKey = configuration["EncryptionKey"];
        }

        public string GenerateShareCode(CheckListPermissionModel checkListPermissions)
        {
            byte[] initialVector = new byte[16];
            var checklistPermissionsToJson = JsonSerializer.Serialize(checkListPermissions);
            byte[] encryptedChecklistJsonToByteArray;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_secretKey);
                aes.IV = initialVector;

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(checklistPermissionsToJson);
                        }

                        encryptedChecklistJsonToByteArray = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encryptedChecklistJsonToByteArray);
        }

        public CheckListPermissionModel GetCheckListDetailsFromCode(string code )
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(code);
            string checkListPermissionsJson;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_secretKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            checkListPermissionsJson= streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return JsonSerializer.Deserialize<CheckListPermissionModel>(checkListPermissionsJson);
        }
    }
}
