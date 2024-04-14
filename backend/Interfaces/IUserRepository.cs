namespace backend.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExists(string id);
}