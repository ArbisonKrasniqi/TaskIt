using Domain.Entities;
using Microsoft.VisualBasic.CompilerServices;

namespace Application.Dtos.CommentDtos;

public class CommentDto
{
    public int CommentId { get; set; }
    public string Content { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TaskId { get; set; }
    public DateTime DateCreated { get; set; }

    public CommentDto(Comment comment)
    {
        CommentId = comment.CommentId;
        Content = comment.Content;
        UserId = comment.UserId;
        TaskId = comment.TaskId;
        DateCreated = comment.DateCreated;
        FirstName = comment.User.FirstName;
        LastName = comment.User.LastName;
    }
}