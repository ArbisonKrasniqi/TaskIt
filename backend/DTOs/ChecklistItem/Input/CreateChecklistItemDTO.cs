using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.ChecklistItem.Input;

public class CreateChecklistItemDTO
{
    [Required]
    [MinLength(2, ErrorMessage = "Content must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]
    public string Content { get; set; } = string.Empty;
    public int ChecklistId { get; set; }
}