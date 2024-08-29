using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Checklist.Input;

public class UpdateChecklistDTO
{
    public int ChecklistId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [Range(0,int.MaxValue, ErrorMessage = "TaskId can not be negative")]
    public int TaskId { get; set; }
}