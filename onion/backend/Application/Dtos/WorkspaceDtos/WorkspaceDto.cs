using Domain.Entities;

namespace Application.Dtos.WorkspaceDtos;

public class WorkspaceDto
{
    public int WorkspaceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string OwnerId { get; set; }
    public List<Members> Members { get; set; }
    // public List<Board> Boards { get; set; }

    //Constructor
    public WorkspaceDto(Workspace workspace)
    {
        WorkspaceId = workspace.WorkspaceId;
        Title = workspace.Title;
        Description = workspace.Description;
        OwnerId = workspace.OwnerId;
        Members = workspace.Members;

    }
}