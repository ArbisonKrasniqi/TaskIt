using Domain.Entities;

namespace Domain.Interfaces;

public interface IInviteRepository
{
    Task<IEnumerable<Invite>> GetInvites(
        string inviteId = null,
        int? workspaceId = null,
        string inviterId = null,
        string inviteeId = null,
        string inviteStatus = null
    );

    Task<Invite> CreateInvite(Invite invite);
    Task<Invite> UpdateInvite(Invite invite);
    Task<Invite> DeleteInvite(int inviteId);
}