using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Checklist
{
    public int ChecklistId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public int TaskId { get; set; }
    public List<ChecklistItem>? ChecklistItems { get; set; }
}