using System.Text.RegularExpressions;

namespace FlightDocumentManagementSystem.Helpers
{
    public class Check
    {
        public static bool IsEmailCompany(string email)
        {
            // Regular expression to check format email@vietjetair.com
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@vietjetair\.com$";

            // Check email format using Regex.IsMatch()
            bool isValid = Regex.IsMatch(email, emailPattern);

            return isValid;
        }

        public static bool IsEmail(string email)
        {
            // Regular expression to check format email@vietjetair.com
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check email format using Regex.IsMatch()
            bool isValid = Regex.IsMatch(email, emailPattern);

            return isValid;
        }

        public static bool IsPhone(string phone)
        {
            string phonePattern = @"^\d{10}$";

            // Check email format using Regex.IsMatch()
            bool isValid = Regex.IsMatch(phone, phonePattern);

            return isValid;
        }
    }
}
