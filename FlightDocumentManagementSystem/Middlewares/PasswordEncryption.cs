using System.Security.Cryptography;
using System.Text;

namespace FlightDocumentManagementSystem.Middlewares
{
    public class PasswordEncryption
    {
        public static string EncryptPassword(string password, string salt)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
                Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, passwordWithSaltBytes, passwordBytes.Length, saltBytes.Length);

                byte[] hashBytes = sha512.ComputeHash(passwordWithSaltBytes);
                string hashedPassword = salt + "." + Convert.ToBase64String(hashBytes);
                return hashedPassword;
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var hp = hashedPassword.Split('.');
            string expectedHashedPassword = EncryptPassword(password, hp[0]);
            return hashedPassword.Equals(expectedHashedPassword);
        }
    }
}
