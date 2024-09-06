namespace backend.DTOs.WorkspaceActivity.Input;

public class AddWorkspaceActivityDto
{
    public int WorkspaceId { get; set; }
    //public int UserId { get; set; }
    public string ActionType { get; set; } // Krijoi, fshiu, perditsoi
    public string EntityName { get; set; } // "Bordi" "Workspace"

}