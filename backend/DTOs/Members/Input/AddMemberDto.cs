using backend.DTOs.User.Input;
using backend.DTOs.Workspace;

namespace backend.DTOs.Members;

public class AddMemberDto
{
    public string UserId { get; set; }
    public int WorkspaceId { get; set; }
}