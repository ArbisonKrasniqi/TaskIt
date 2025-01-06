namespace Application.Dtos.InviteDtos;

public class UpdateInviteAdminDto
{
    public int InviteId { get; set; }
    public int WorkspaceId { get; set; }
    public string InviterId { get; set; }
    public string InviteeId { get; set; }
    public string InviteStatus { get; set; }
}