namespace backend.DTOs.Members.Output;
using backend.Models;

public class MemberDto
{
    public int MemberId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }

}