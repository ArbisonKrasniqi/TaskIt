using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class WorkspaceActivity
{
    [Key]
    public int WorkspaceActivityId { get; set; }
    public int WorkspaceId { get; set; }
    public string UserId { get; set; }
    public string ActionType { get; set; } // Krijoi, fshiu, perditsoi
    public string EntityName { get; set; } // "Bordi" "Workspace"
    public DateTime ActionDate { get; set; }
    
    public Workspace Workspace { get; set; }
    public User User { get; set; }
    
    public WorkspaceActivity(){}
    public WorkspaceActivity(int workspaceId, string userId, string actionType, string entityName, DateTime actionDate)
    {
        WorkspaceId = workspaceId;
        UserId = userId;
        ActionType = actionType;
        EntityName = entityName;
        ActionDate = actionDate;
    }
    
    public WorkspaceActivity(int workspaceActivityId, int workspaceId, string userId, string actionType, string entityName, DateTime actionDate)
    {
        WorkspaceActivityId = workspaceActivityId;
        WorkspaceId = workspaceId;
        UserId = userId;
        ActionType = actionType;
        EntityName = entityName;
        ActionDate = actionDate;
    }
}