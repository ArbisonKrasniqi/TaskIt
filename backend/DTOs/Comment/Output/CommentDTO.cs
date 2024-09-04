namespace backend.DTOs.Comment.Output;

public class CommentDTO
{
    public int CommentId { get; set; }
    public string Content { get; set; }
    public DateTime DateAdded { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TaskId { get; set; }
}