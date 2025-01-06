using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ListDtos;

public class DragNDropListDto
{
    [Required]
    public int ListId { get; set; }
    [Required]
    public int newIndex { get; set; }
}