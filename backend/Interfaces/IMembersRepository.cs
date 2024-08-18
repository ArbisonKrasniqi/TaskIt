using backend.DTOs.Members;
using backend.DTOs.User.Input;
using backend.DTOs.Workspace;

namespace backend.Interfaces;
using backend.Models;

public interface IMembersRepository
{
    Task AddMemberAsync (AddMemberDto addMemberDto);
    Task<List<Members?>> GetAllMembersAsync();
    Task<List<Members>> GetAllMembersByWorkspaceAsync(int workspaceId);
    Task<User> RemoveMemberAsync(int workspaceId, string memberId);
    Task<bool> IsAMember(string userId, int? workspaceId);
    Task<Members?> DeleteMemberById(int id);
    Task<Members?> UpdateMemberAsync(UpdateMemberDto memberDto);

}