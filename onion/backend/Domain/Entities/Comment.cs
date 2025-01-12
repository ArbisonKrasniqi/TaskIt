using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Comment
{
    [Key]
    public int CommentId { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }
    public string UserId { get; set; }
    public int TaskId { get; set; }

    public User User { get; set; }
    public Tasks Task { get; set; }
    
    public Comment(){}

    public Comment(string content, int taskId)
    {
        Content = content;
        TaskId = taskId;
    }
}