using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Invite
{
    [Key] public int InviteId { get; set; }
    public int WorkspaceId { get; set; }
    public string InviterId { get; set; }
    public string InviteeId { get; set; }
    public string InviteStatus { get; set; }
    public DateTime DateSent { get; set; }
    
    //Navigation
    public Workspace Workspace { get; set; }
    public User Inviter { get; set; }
    public User Invitee { get; set; }
    
    
    // Constructors

    public Invite(){}

    public Invite(int inviteId, int workspaceId,string inviterId, string inviteeId, string inviteStatus, DateTime dateSent)
    {
        InviteId = inviteId;
        WorkspaceId = workspaceId;
        InviterId = inviterId;
        InviteeId = inviteeId;
        InviteStatus = inviteStatus;
        DateSent = dateSent;
    }

    public Invite(int workspaceId, string inviterId, string inviteeId)
    {
        WorkspaceId = workspaceId;
        InviterId = inviterId;
        InviteeId = inviteeId;
    }
}