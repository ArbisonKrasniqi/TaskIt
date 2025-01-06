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
}