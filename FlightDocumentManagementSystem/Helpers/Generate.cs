using System.Security.Cryptography;

namespace FlightDocumentManagementSystem.Helpers
{
    public class Generate
    {
        public static string GetSalt()
        {
            byte[] saltBytes = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            return salt;
        }

        public static string GetRefreshToken()
        {
            var random = new byte[32];
            using (var ran = RandomNumberGenerator.Create())
            {
                ran.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
    }
}
