using backend.DTOs.User.Input;
using backend.DTOs.Workspace;

namespace backend.Interfaces;
using backend.Models;

public interface IMembersRepository
{
    Task AddMemberAsync (UserIdDTO userId, WorkspaceIdDto workspaceId);
    Task<List<User>> GetAllMembersAsync(int workspaceId);
    Task<User> RemoveMemberAsync(UserIdDTO userId, WorkspaceIdDto workspaceId);
}