using System.ComponentModel.DataAnnotations;
using backend.DTOs.ChecklistItem.Output;

namespace backend.DTOs.Checklist.Output;

public class ChecklistDTO
{
    public int ChecklistId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    [Range(0,int.MaxValue,ErrorMessage = "TaskId can not be negative")]
    public int? TaskId { get; set; }
    public List<ChecklistItemDTO> ChecklistItems { get; set; }
}