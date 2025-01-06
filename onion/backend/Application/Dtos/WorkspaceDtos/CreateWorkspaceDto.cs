using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.WorkspaceDtos;

public class CreateWorkspaceDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
}