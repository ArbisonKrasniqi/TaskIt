using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.BoardDtos;

public class CreateBoardDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public int WorkspaceId { get; set; }
}