using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.WorkspaceDtos;

public class WorkspaceIdDto
{
    [Required]
    public int WorkspaceId { get; set; }
}