using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightDocumentManagementSystem.Middlewares
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailLogRepository _emailLogRepository;

        public AuthsController(IAccountRepository accountRepository,
            IAuthRepository authRepository,
            IRoleRepository roleRepository,
            IConfiguration configuration,
            IEmailLogRepository emailLogRepository)
        {
            _accountRepository = accountRepository;
            _authRepository = authRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _emailLogRepository = emailLogRepository;
        }

        // POST: api/Auths/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] Auth auth)
        {
            var isCorrect = await _accountRepository.CheckLoginAsync(auth);
            if (isCorrect == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Email or Password is incorrect",
                    Data = null
                });
            }

            var account = await _accountRepository.GetAccountByEmailAsync(auth.Email ?? "");
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Email error",
                });
            }

            var role = await _roleRepository.FindRoleByIdAsync(account.RoleId);
            if (role == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Email Role error",
                });
            }
            await _authRepository.DeleteTokenAsync(account);
            var result = await _authRepository.InsertTokenAsync(account, role);

            return Ok(new Notification
            {
                Success = true,
                Message = "Account is correct",
                Data = result
            });
        }

        // POST: api/Auths/RefreshToken
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(Token tokens)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "")),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = handler.ValidateToken(tokens.AccessToken, tokenValidateParam, out var validatedToken);

                //Check Valid Type
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        return Ok(new
                        {
                            Message = "Invalid token"
                        });
                    }
                }

                //Check accessToken expire?
                var utcExpireDate = tokenInVerification.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
                if (utcExpireDate == null)
                {
                    return Ok(new
                    {
                        Message = "Can't find Time Exp"
                    });
                }
                var expireDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(utcExpireDate));
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new
                    {
                        Message = "Access token has not yet expired"
                    });
                }

                //Check refreshtoken exist in DB
                var storedToken = await _authRepository.GetTokenAsync(tokens.RefreshToken ?? "");
                if (storedToken == null)
                {
                    return Ok(new
                    {
                        Message = "Refresh token does not exist"
                    });
                }

                //Check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return Ok(new
                    {
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new
                    {
                        Message = "Refresh token has been revoked"
                    });
                }
                await _authRepository.UpdateTokenAsync(storedToken);

                //create new token
                var account = await _accountRepository.FindAccountByIdAsync(storedToken.AccountId);
                if (account == null)
                {
                    return Ok(new
                    {
                        Message = "Invalid account"
                    });
                }

                var rolename = await _roleRepository.FindRoleByIdAsync(account.RoleId);
                if (rolename == null)
                {
                    return Ok(new
                    {
                        Message = "Invalid role"
                    });
                }

                var token = await _authRepository.InsertTokenAsync(account, rolename);

                return Ok(new
                {
                    Message = "Refresh token success",
                    Token = token
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Message = "Something went wrong"
                });
            }
        }

        // POST: api/Auths/Logout
        [Authorize]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            return Ok(new
            {
                message = "Logout success"
            });
        }

        // POST: api/Auths/ForgotPassword
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> PostForgotPassword(string email)
        {
            var account = await _accountRepository.GetAccountByEmailAsync(email);
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Invalid Email",
                    Data = null
                });
            }
            var emailLogId = await _emailLogRepository.InsertEmailLogAsync(account.AccountId);
            var emailHtml = Email.EmailResetPasswordContent(account.Email!, emailLogId);
            Email.SendEmail(account.Email!, "Reset Password", emailHtml);

            return Ok(new Notification
            {
                Success = true,
                Message = $"Sent to email {email} successfully",
                Data = null
            });
        }

        // POST: api/Auths/ChangePassword
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<ActionResult> PostChangePassword(string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(oldPassword))
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Please enter all information",
                    Data = null
                });
            }
            var account = await _accountRepository.FindAccountByIdAsync(Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (account == null)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "This Account doesn't exist or invalid",
                    Data = null
                });
            }
            if (PasswordEncryption.VerifyPassword(oldPassword, account.Password!) == false)
            {
                return Ok(new Notification
                {
                    Success = false,
                    Message = "Old password is incorrect",
                    Data = null
                });
            }
            var emailLogId = await _emailLogRepository.InsertEmailLogAsync(account.AccountId);
            var emailHtml = Email.EmailChangePasswordContent(account.Email!, newPassword, emailLogId);
            Email.SendEmail(account.Email!, "Reset Password", emailHtml);
            return Ok(new Notification
            {
                Success = true,
                Message = $"Sent to email {account.Email} successfully",
                Data = null
            });
        }

        // GET: api/Auths/ResetPassword
        [HttpGet("ResetPassword")]
        public async Task<ActionResult> GetResetPassword()
        {
            string input = Request.QueryString.ToString();
            if (input.Length > 0)
            {
                string[] parameters = input.TrimStart('?').Split('&');

                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                foreach (string parameter in parameters)
                {
                    string[] keyValue = parameter.Split('=');
                    string key = keyValue[0];
                    string value = keyValue[1];
                    queryParams.Add(key, value);
                }

                string email = queryParams["email"];
                string emailLogId = queryParams["id"];

                var account = await _accountRepository.GetAccountByEmailAsync(email);
                if (account == null)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = $"Email {email} Error",
                        Data = null
                    });
                }

                var emailLog = await _emailLogRepository.FindEmailLogByIdAsync(Guid.Parse(emailLogId));
                if (emailLog == null)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = $"Error Reset Password",
                        Data = null
                    });
                }

                var status = await _emailLogRepository.CheckVerifyAsync(emailLog.EmailLogId, account.AccountId);
                if (status == null)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = $"Error Link",
                        Data = null
                    });
                }
                else if (status == true)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = $"This link is no longer available",
                        Data = null
                    });
                }

                string password = email.Substring(0, email.IndexOf('@'));
                await _accountRepository.UpdatePasswordAsync(account, password);
                await _emailLogRepository.UpdateEmailLogAsync(emailLog);
                return Ok(new Notification
                {
                    Success = true,
                    Message = $"Email {email} password reset successfully",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = $"Please choose forgot password",
                Data = null
            });
        }

        // GET: api/Auths/UpdatePassword
        [HttpGet("UpdatePassword")]
        public async Task<ActionResult> GetUpdatePassword()
        {
            string input = Request.QueryString.ToString();
            if (input.Length > 0)
            {
                string[] parameters = input.TrimStart('?').Split('&');

                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                foreach (string parameter in parameters)
                {
                    string[] keyValue = parameter.Split('=');
                    string key = keyValue[0];
                    string value = keyValue[1];
                    queryParams.Add(key, value);
                }

                string email = queryParams["email"];
                string emailLogId = queryParams["id"];
                string newPassword = queryParams["password"];

                var account = await _accountRepository.GetAccountByEmailAsync(email);
                if (account == null)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = $"Email {email} Error",
                        Data = null
                    });
                }

                var emailLog = await _emailLogRepository.FindEmailLogByIdAsync(Guid.Parse(emailLogId));
                if (emailLog == null)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = $"Error Reset Password",
                        Data = null
                    });
                }

                var status = await _emailLogRepository.CheckVerifyAsync(emailLog.EmailLogId, account.AccountId);
                if (status == null)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = $"Error Link",
                        Data = null
                    });
                }
                else if (status == true)
                {
                    return Ok(new Notification
                    {
                        Success = false,
                        Message = $"This link is no longer available",
                        Data = null
                    });
                }

                await _accountRepository.UpdatePasswordAsync(account, newPassword);
                await _emailLogRepository.UpdateEmailLogAsync(emailLog);
                return Ok(new Notification
                {
                    Success = true,
                    Message = $"Email {email} password changed successfully",
                    Data = null
                });
            }
            return Ok(new Notification
            {
                Success = true,
                Message = $"Please choose change password",
                Data = null
            });
        }
    }
}
