using Domain.Entities;

namespace Application.Dtos.WorkspaceActivityDto;

public class WorkspaceActivityDto
{
    public int WorkspaceActivityId { get; set; }
    public int WorkspaceId { get; set; }
    public string UserId { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string ActionType { get; set; } 
    public string EntityName { get; set; } 
    public DateTime ActionDate { get; set; }

    public WorkspaceActivityDto(WorkspaceActivity workspaceActivity)
    {
        WorkspaceActivityId = workspaceActivity.WorkspaceActivityId;
        WorkspaceId = workspaceActivity.WorkspaceId;
        UserId = workspaceActivity.UserId;
        ActionType = workspaceActivity.ActionType;
        EntityName = workspaceActivity.EntityName;
        ActionDate = workspaceActivity.ActionDate;
        UserFirstName = workspaceActivity.User.FirstName;
        UserLastName = workspaceActivity.User.LastName;
    }
}