using Domain.Interfaces;

namespace Application.Handlers.Invite;

public class InviteDeleteHandler : IInviteDeleteHandler
{
    private readonly IInviteRepository _inviteRepository;

    public InviteDeleteHandler(IInviteRepository inviteRepository)
    {
        _inviteRepository = inviteRepository;
    }

    public async Task HandleDeleteRequest(int inviteId)
    {
        await _inviteRepository.DeleteInvite(inviteId);
    }
}