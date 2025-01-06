using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ListDtos;

public class CreateListDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public int BoardId { get; set; }
}