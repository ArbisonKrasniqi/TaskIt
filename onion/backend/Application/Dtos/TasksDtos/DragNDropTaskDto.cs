using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.TasksDtos;

public class DragNDropTaskDto
{
    [Required]
    public int TaskId { get; set; }
    [Required]
    public int newIndex { get; set; }
    [Required]
    public int ListId { get; set; }
}