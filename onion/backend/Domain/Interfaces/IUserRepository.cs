using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers(
        string userId = null,
        string firstName = null,
        string lastName = null,
        string email = null,
        DateTime? dateCreated = null,
        bool? isDeleted = null,
        string role = null,
        string refreshToken = null
        );
    Task<User> CreateUser(User user);
    Task<User> UpdateUser(User user);
    Task<User> DeleteUser(string userId);
}