namespace Application.Dtos.InviteDtos;

public class InviteInfoDto
{
    public int InviteId { get; set; }
    public int WorkspaceId { get; set; }
    public string InviterId { get; set; }
    public string InviteeId { get; set; }
    public string InviteStatus { get; set; }
    public DateTime DateSent { get; set; }
    
    //Constructor
    public InviteInfoDto(int inviteId, int workspaceId, string inviterId, string inviteeId, string inviteStatus, DateTime dateSent)
    {
        InviteId = inviteId;
        WorkspaceId = workspaceId;
        InviterId = inviterId;
        InviteeId = inviteeId;
        InviteStatus = inviteStatus;
        DateSent = dateSent;
    }
}