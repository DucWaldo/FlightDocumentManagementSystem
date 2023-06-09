using FlightDocumentManagementSystem.Middlewares;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IAuthRepository : IRepository<TokenManager>
    {
        public Task<Token> InsertTokenAsync(Account account, Role role);
        public Task<TokenManager?> GetTokenAsync(string refreshToken);
        public Task UpdateTokenAsync(TokenManager token);
        public Task DeleteTokenAsync(Account account);
    }
}
