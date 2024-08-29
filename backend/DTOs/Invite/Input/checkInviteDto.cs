namespace backend.DTOs.Invite.Input;

public class checkInviteDto
{
    public int WorkspaceId { get; set; }
    public string InviterId { get; set; } = string.Empty;
    public string InviteeId { get; set; } = string.Empty;
}