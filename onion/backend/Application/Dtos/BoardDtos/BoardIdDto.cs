using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.BoardDtos;

public class BoardIdDto
{
    [Required]
    public int BoardId { get; set; }
}