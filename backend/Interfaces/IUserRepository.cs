using backend.DTOs.User.Output;

namespace backend.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExists(string id);
    Task<bool> UserOwnsWorkspace(string userId, int? workspaceId);
    Task<bool> UserIsMember(string userId, int workspaceId);
    Task<List<SearchUserDto>> SearchUsersAsync(string query);
}