namespace SharedListApi.Services
{
    public  static class UserIdService
    {

        private static string str => "abcdefghijklmnopqrstuvwyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateUserId()
        {
            int userIdLength = 16;
            Random random = new Random();
            string userId = "";

            for(int i = 0; i< userIdLength; i++)
            {
                int randomIndex = random.Next(str.Length);
                userId = userId + str[randomIndex]; 
            }


            return userId;
        }

    }
}
