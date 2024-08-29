using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace backend.DTOs.Comment;

public class CreateCommentRequestDTO
{
    [Required]
    [MinLength(5, ErrorMessage="Content must be atleast 5 characaters")]
    [MaxLength(280, ErrorMessage = "Comment cannot be over 280 characters")]
    public string Content { get; set; }
    [Required]
    public int TaskId { get; set; }
}