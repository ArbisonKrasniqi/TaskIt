namespace backend.Interfaces;
using backend.DTOs.Invite.Input;
using backend.Models;
public interface IInviteRepository
{
    Task<List<Invite>> GetAllInvitesAsync();
    Task<Invite?> GetInviteByIdAsync(int id);
    Task<List<Invite?>> GetInvitesByInviterAsync(string inviterId);
    Task<List<Invite?>> GetInvitesByInviteeAsync(string inviteeId);
    Task<List<Invite>> GetInvitesByWorkspaceAsync(int id);
    Task<List<Invite>> GetPendingInvitesByInviter(string userId);
    Task<List<Invite>> GetPendingInvitesByInvitee(string userId);
    Task<bool> InviteExistsAsync(string inviterId, string inviteeId, int workspaceId);
    Task<Invite> AddInviteAsync(Invite invite);
    Task<Invite?> UpdateInviteStatusAsync(UpdateInviteDto updateInviteDto);
    Task<bool> DeleteInviteAsync(int inviteId);
    Task<Invite?> UpdateInviteAsync(UpdateInviteAdminDto updateDto);
}