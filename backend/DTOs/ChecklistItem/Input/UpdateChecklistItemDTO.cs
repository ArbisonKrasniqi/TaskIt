using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.ChecklistItem.Input;

public class UpdateChecklistItemDTO
{
    public int ChecklistItemId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Content must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]
    public string Content { get; set; } = string.Empty;
    [Required]
    [Range(0,int.MaxValue, ErrorMessage = "ChecklistId can not be negative")]
    public int ChecklistId { get; set; }
}