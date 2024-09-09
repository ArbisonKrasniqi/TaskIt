using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Comment
{
    [Key]
    public int CommentId { get; set; }
    public string Content { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserId { get; set; }
    public int TaskId { get; set; }

    public User User { get; set; }
    public Tasks Task { get; set; }
}