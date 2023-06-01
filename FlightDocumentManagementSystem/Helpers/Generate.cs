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

        public static string GetStaffCode()
        {
            DateTime current = DateTime.UtcNow;
            int currentYear = current.Year % 100;
            int currentDay = current.Day;
            int currentMonth = current.Month;
            int currentHour = current.Hour;
            int currentMinute = current.Minute;
            int currentSecond = current.Second;
            int currentMillisecond = current.Millisecond / 10;

            //00xxxxxxxx
            int first = currentYear;
            //xx00xxxxxx
            int second = 0;
            if (currentDay >= currentMonth)
            {
                second = (currentDay + currentMonth + GetRandom(1));
            }
            else
            {
                second = (currentDay + currentMonth) * 2 + GetRandom(1);
            }
            //xxxx000xxx
            int third = (currentHour + currentMinute + currentSecond + currentMillisecond + GetRandom(2));
            if (third < 100)
            {
                third = third * 10;
            }
            //xxxxxxx000
            int fourth = GetRandom(3);
            string result = first.ToString() + second.ToString() + third.ToString() + fourth.ToString();
            return result;
        }

        public static int GetRandom(int number)
        {
            Random random = new Random();
            switch (number)
            {
                case 1:
                    {
                        int verificationCode = random.Next(1, 9);
                        return verificationCode;
                    }
                case 2:
                    {
                        int verificationCode = random.Next(10, 99);
                        return verificationCode;
                    }
                case 3:
                    {
                        int verificationCode = random.Next(100, 999);
                        return verificationCode;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }
    }
}
