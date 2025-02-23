﻿namespace backend.DTOs.WorkspaceActivity.Output;

public class WorkspaceActivityDto
{
    public int WorkspaceActivityId { get; set; }
    public int WorkspaceId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string UserLastName { get; set; }
    public string ActionType { get; set; } // Krijoi, fshiu, perditsoi
    public string EntityName { get; set; } // "Bordi" "Workspace"
    public DateTime ActionDate { get; set; }
}