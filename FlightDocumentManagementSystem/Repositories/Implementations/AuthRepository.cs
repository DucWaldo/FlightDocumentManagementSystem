using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Middlewares;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightDocumentManagementSystem.Repositories.Implementations
{
    public class AuthRepository : RepositoryBase<TokenManager>, IAuthRepository
    {
        private readonly IConfiguration _configuration;
        public AuthRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public async Task<TokenManager?> GetTokenAsync(string refreshToken)
        {
            var result = await _dbSet.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
            return result;
        }

        public async Task<Token> InsertTokenAsync(Account account, Role role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString() ?? ""),
                    new Claim(ClaimTypes.Role, role.Name ?? "")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = Generate.GetRefreshToken();

            var tokenManager = new TokenManager
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),
                AccountId = account.AccountId
            };

            await InsertAsync(tokenManager);

            return new Token
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task UpdateTokenAsync(TokenManager token)
        {
            token.IsRevoked = true;
            token.IsUsed = true;
            await UpdateAsync(token);
        }
    }
}
