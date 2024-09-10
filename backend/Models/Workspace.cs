using backend.DTOs;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Workspace
{
    //Attributes
    public int WorkspaceId { get; set; }
    
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(25, ErrorMessage = "Title cannot be over 25 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
    [MaxLength(280, ErrorMessage = "Description cannot be over 280 characters")]
    public string Description { get; set; } = string.Empty;

    public string OwnerId { get; set; }
    public User User { get; set; }
    public List<Board> Boards { get; set; } = new List<Board>();
    public List<Members> Members { get; set; } = new List<Members>();


}