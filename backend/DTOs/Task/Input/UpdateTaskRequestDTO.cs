namespace backend.DTOs.Task;
using System.ComponentModel.DataAnnotations;

public class UpdateTaskRequestDTO{
    
    public int TaskId { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
    [MaxLength(280, ErrorMessage = "Description cannot be over 280 characters")]
    public string Description { get; set; } = string.Empty;

    
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "List Id can not be negative!")]
    public int ListId { get; set;}

    
}