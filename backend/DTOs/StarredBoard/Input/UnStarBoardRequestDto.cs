using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.StarredBoard;

public class UnStarBoardRequestDto
{
    [Required]
    public int BoardId { get; set; } 
    [Required]
    public string UserId { get; set; }
    public int WorkspaceId { get; set; }
}