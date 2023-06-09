using FlightDocumentManagementSystem.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace FlightDocumentManagementSystem.Helpers
{
    public static class Email
    {
        private static IConfiguration _configuration;
        private static string host;
        private static string name;
        private static string email;
        private static string password;
        private static string subject;
        static Email()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            host = _configuration["Mail:Host"]!;
            name = _configuration["Mail:Name"]!;
            email = _configuration["Mail:Email"]!;
            password = _configuration["Mail:Password"]!;
            subject = _configuration["Mail:Subject"]!;
        }

        public static void SendEmail(string recipient, Account account)
        {
            string htmlBody = EmailContent(account);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(name, email));
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlBody;


            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(host, 587, SecureSocketOptions.StartTls);
                client.Authenticate(email, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public static string EmailContent(Account account)
        {
            var resetPasswordLink = $"https://localhost:7099/api/Auths/ResetPassword?email={account.Email}";
            string emailContent = $@"<!DOCTYPE html>
                                <html>
                                <head>
                                  <title>Đặt lại mật khẩu</title>
                                </head>
                                <body>
                                  <h1>Đặt lại mật khẩu</h1>
                                  <p>Xin chào, {account.Email},</p>
                                  <p>Bạn đã yêu cầu đặt lại mật khẩu của tài khoản. Nhấn vào liên kết bên dưới để đặt lại mật khẩu:</p>
                                  <p><a href=""{resetPasswordLink}"">Đặt lại mật khẩu</a></p>
                                  <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                                  <p>Trân trọng,</p>
                                  <p>Ban quản trị</p>
                                </body>
                                </html>";

            return emailContent;
        }
    }
}
