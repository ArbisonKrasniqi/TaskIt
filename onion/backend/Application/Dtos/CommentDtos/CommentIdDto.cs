using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.CommentDtos;

public class CommentIdDto
{
    [Required]
    public int CommentId { get; set; }
}