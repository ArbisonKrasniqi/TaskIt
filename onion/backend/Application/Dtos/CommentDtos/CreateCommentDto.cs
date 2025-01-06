using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.CommentDtos;

public class CreateCommentDto
{
    [Required]
    public string Content { get; set; }
    [Required]
    public int TaskId { get; set; }
}