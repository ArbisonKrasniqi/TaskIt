namespace backend.DTOs.Invite.Input;

public class UpdateInviteDto
{
    public int InviteId { get; set; }
    public string InviteStatus { get; set; } = string.Empty;
}