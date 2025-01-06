using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.CommentDtos;

public class UpdateCommentDto
{
    [Required]
    public int CommentId { get; set; }
    [Required]
    public string Content { get; set; }
}