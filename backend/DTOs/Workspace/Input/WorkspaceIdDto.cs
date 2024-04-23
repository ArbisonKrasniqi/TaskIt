using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Workspace;

public class WorkspaceIdDto
{
    [Required]
    public int WorkspaceId { get; set; }
}