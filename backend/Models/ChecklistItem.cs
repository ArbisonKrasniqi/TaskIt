using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class ChecklistItem
{
    public int ChecklistItemId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Content must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]
    public string Content { get; set; } = string.Empty;
    public bool Checked { get; set; } 
    public DateTime DateAdded { get; set; }
    public int ChecklistId { get; set; }
    
}