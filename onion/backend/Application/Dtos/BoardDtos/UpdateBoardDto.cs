using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.BoardDtos;

public class UpdateBoardDto
{
    [Required]
    public int BoardId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public int WorkspaceId { get; set; }
}