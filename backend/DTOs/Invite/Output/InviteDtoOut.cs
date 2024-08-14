namespace backend.DTOs.Invite.Output;

public class InviteDtoOut

{   public int InviteId { get; set; }
    public int WorkspaceId { get; set; }
    public string InviterId { get; set; } = string.Empty;
    public string InviteeId { get; set; } = string.Empty;
    public string InviteStatus { get; set; } = string.Empty;
    public DateTime DateSent { get; set; } 
}