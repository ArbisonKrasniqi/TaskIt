using backend.DTOs.User.Input;
using backend.DTOs.Workspace;

namespace backend.DTOs.Members;

public class RemoveMemberDto
{
    public string UserId { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }

}