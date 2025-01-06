using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ListDtos;

public class UpdateListDto
{
    [Required]
    public int ListId { get; set; }
    [Required]
    public string Title { get; set; }
}