using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Comment;

public class UpdateCommentDTO
{
    [Required]
    public int CommentId { get; set; }
    [Required]
    [MinLength(5, ErrorMessage="Content must be atleast 5 characaters")]
    [MaxLength(280, ErrorMessage = "Comment cannot be over 280 characters")]
    public string Content { get; set; }
}