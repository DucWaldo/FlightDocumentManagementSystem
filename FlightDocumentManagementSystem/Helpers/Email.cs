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

        public static void SendEmail(string recipient, string funcString, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(name, email));
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = funcString + " - " + subject;

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

        public static string EmailResetPasswordContent(string email, string emailLogId)
        {
            var resetPasswordLink = $"https://localhost:7099/api/Auths/ResetPassword?email={email}&id={emailLogId}";
            var logoLink = Cloudinary.GetImageUrl("FlightDocumentManagementSystem/Logo");
            string emailContent = $@"   <!DOCTYPE html>
                                        <html>
                                        <head>
                                            <title>Reset Password</title>
                                            <style>
                                            .header {{
                                                text-align: center;
                                                margin-bottom: 20px;
                                            }}
                                            .logo-img {{
                                                max-width: 200px;
                                            }}
                                            .button-container {{
                                                text-align: center;
                                                margin-top: 20px;
                                            }}
                                            .button-container a {{
                                                background-color: #4CAF50;
                                                border: none;
                                                color: white;
                                                padding: 15px 32px;
                                                text-align: center;
                                                text-decoration: none;
                                                display: inline-block;
                                                font-size: 16px;
                                                margin: 4px 2px;
                                                cursor: pointer;
                                            }}
                                            </style>
                                        </head>
                                        <body>
                                            <div class=""header"">
                                            <img src=""{logoLink}"" alt=""Logo"" class=""logo-img"">
                                            </div>
                                            <h1>Reset Password</h1>
                                            <p>Hello, {email},</p>
                                            <p>You have requested to reset the password for your account. Click the button below to reset your password:</p>
                                            <div class=""button-container""><a href=""{resetPasswordLink}"">Reset Password</button></a></div>
                                            <p>If you did not request to reset your password, please ignore this email.</p>
                                            <p>Regards,</p>
                                            <p>Admin</p>
                                        </body>
                                        </html>";
            return emailContent;
        }

        public static string EmailChangePasswordContent(string email, string newPassword, string emailLogId)
        {
            var resetPasswordLink = $"https://localhost:7099/api/Auths/UpdatePassword?email={email}&password={newPassword}&id={emailLogId}";
            var logoLink = Cloudinary.GetImageUrl("FlightDocumentManagementSystem/Logo");
            string emailContent = $@"   <!DOCTYPE html>
                                        <html>
                                        <head>
                                            <title>Change Password</title>
                                            <style>
                                            .header {{
                                                text-align: center;
                                                margin-bottom: 20px;
                                            }}
                                            .logo-img {{
                                                max-width: 200px;
                                            }}
                                            .button-container {{
                                                text-align: center;
                                                margin-top: 20px;
                                            }}
                                            .button-container a {{
                                                background-color: #4CAF50;
                                                border: none;
                                                color: white;
                                                padding: 15px 32px;
                                                text-align: center;
                                                text-decoration: none;
                                                display: inline-block;
                                                font-size: 16px;
                                                margin: 4px 2px;
                                                cursor: pointer;
                                            }}
                                            </style>
                                        </head>
                                        <body>
                                            <div class=""header"">
                                            <img src=""{logoLink}"" alt=""Logo"" class=""logo-img"">
                                            </div>
                                            <h1>Change Password</h1>
                                            <p>Hello, {email},</p>
                                            <p>You have requested to change the password for your account. Click the button below to change your password:</p>
                                            <div class=""button-container""><a href=""{resetPasswordLink}"">Change Password</button></a></div>
                                            <p>If you did not request to change your password, please ignore this email.</p>
                                            <p>Regards,</p>
                                            <p>Admin</p>
                                        </body>
                                        </html>";
            return emailContent;
        }
    }
}
