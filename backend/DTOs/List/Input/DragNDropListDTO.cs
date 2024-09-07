using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.List;

public class DragNDropListDTO
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "List id can not be negative!")]
    public int BoardId { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "List id can not be negative!")]
    public int oldIndex { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "New index can not be negative!")]
    public int newIndex { get; set; }
}