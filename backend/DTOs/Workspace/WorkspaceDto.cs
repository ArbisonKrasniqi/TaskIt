namespace backend.DTOs.Workspace;

public class WorkspaceDto
{
    public int WorkspaceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int UserId { get; set; }
}