namespace backend.DTOs.Members;

public class UpdateMemberDto
{
    public int MemberId { get; set; }
    public string UserId { get; set; }
    public DateTime DateJoined { get; set; }
    public int WorkspaceId { get; set; }
}