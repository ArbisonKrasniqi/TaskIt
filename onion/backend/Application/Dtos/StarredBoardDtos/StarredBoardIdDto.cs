using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.StarredBoardDtos;

public class StarredBoardIdDto
{
    [Required]
    public int StarredBoardId { get; set; }
}