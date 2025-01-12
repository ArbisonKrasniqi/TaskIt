namespace Application.Dtos.InviteDtos;

public class CreateInviteDto
{
    public int WorkspaceId { get; set; }
    public string InviterId { get; set; }
    public string InviteeId { get; set; }

    public CreateInviteDto(int workspaceId, string inviterId, string inviteeId)
    {
        WorkspaceId = workspaceId;
        InviterId = inviterId;
        InviteeId = inviteeId;
    }
}