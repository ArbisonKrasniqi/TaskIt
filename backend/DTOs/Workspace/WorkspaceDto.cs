namespace backend.DTOs.Workspace;

public class WorkspaceDto
{
    public int WorkspaceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public string OwnerId { get; set; }
}