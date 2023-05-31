using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

        public AuthsController(IAccountRepository accountRepository,
            IAuthRepository authRepository,
            IRoleRepository roleRepository,
            IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _authRepository = authRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
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

            var result = await _authRepository.InsertTokenAsync(account, role);

            return Ok(new Notification
            {
                Success = true,
                Message = "Account is correct",
                Data = result
            });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(Token tokens)
        {
            var handler = new JwtSecurityTokenHandler();
            //var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");
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

        [Authorize]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            return Ok(new
            {
                message = "Logout success"
            });
        }
    }
}
