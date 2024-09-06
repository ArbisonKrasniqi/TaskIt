using System.ComponentModel.DataAnnotations;
using backend.DTOs.Checklist.Output;

namespace backend.DTOs.Task;

public class TaskDtoLabels
{
    public int TaskId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "List Id can not be negative!")]
    public int ListId { get; set; }
    public DateTime DueDate { get; set; }
    
    public List<ChecklistDTO> Checklists { get; set; }
    public List<Models.Label> Labels { get; set; }

    public DateTime DateAdded { get; set; }
}