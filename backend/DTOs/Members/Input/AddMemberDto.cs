using backend.DTOs.User.Input;
using backend.DTOs.Workspace;

namespace backend.DTOs.Members;

public class AddMemberDto
{
    public string UserId { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
}