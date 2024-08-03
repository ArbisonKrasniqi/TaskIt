namespace backend.DTOs.Workspace;
using System.ComponentModel.DataAnnotations;

public class UpdateWorkspaceRequestDto
{
    public int WorkspaceId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
    [MaxLength(280, ErrorMessage = "Description cannot be over 280 characters")]
    public string Description { get; set; } = string.Empty;

    public string OwnerId { get; set; } = string.Empty;
}