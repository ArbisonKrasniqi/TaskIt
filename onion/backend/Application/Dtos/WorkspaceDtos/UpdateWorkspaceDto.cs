using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.WorkspaceDtos;

public class UpdateWorkspaceDto
{
    [Required]
    public int WorkspaceId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string OwnerId { get; set; }
}