using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class List
{
    public int ListId { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Index value can not be negative!")]
    public int index { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Board Id can not be negative!")]
    
    public int BoardId { get; set; }

    public List<Tasks> Tasks { get; set; } = new List<Tasks>();
}