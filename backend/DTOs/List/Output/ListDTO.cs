using System.ComponentModel.DataAnnotations;
using backend.DTOs.Task;

namespace backend.DTOs.List;

public class ListDTO
{
    public int ListId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Board Id can not be negative!")]
    public int BoardId { get; set; }
    public List<TaskDto> Tasks { get; set; }
    
    public DateTime DateCreated { get; set; }
}