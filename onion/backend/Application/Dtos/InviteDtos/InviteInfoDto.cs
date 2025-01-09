using Domain.Entities;

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
    public InviteInfoDto(Invite invite)
    {
        InviteId = invite.InviteId;
        WorkspaceId = invite.WorkspaceId;
        InviterId = invite.InviterId;
        InviteeId = invite.InviteeId;
        InviteStatus = invite.InviteStatus;
        DateSent = invite.DateSent;
    }
}