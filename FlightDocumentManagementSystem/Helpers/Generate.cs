using System.Security.Cryptography;

namespace FlightDocumentManagementSystem.Helpers
{
    public class Generate
    {
        public static byte[] GetSalt()
        {
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
