using FlightDocumentManagementSystem.Data;
using FlightDocumentManagementSystem.Models;

namespace FlightDocumentManagementSystem.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> FindUserByIdAsync(Guid id);
        public Task<User> InsertUserAsync(UserDTO user);
        public Task<User> UpdateUserAsync(User oldUser, UserDTO newUser);
        public Task DeleteUserAsync(User user);
        public bool ValidateUserDTO(UserDTO user);
    }
}
