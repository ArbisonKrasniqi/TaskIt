using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.StarredBoardDtos;

public class CreateStarredBoardDto
{
    [Required]
    public int BoardId { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public int WorkspaceId { get; set; }
}