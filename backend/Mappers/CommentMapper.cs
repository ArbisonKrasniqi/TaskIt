using backend.DTOs.Comment;
using backend.DTOs.Comment.Output;
using backend.Models;

namespace backend.Mappers;

public static class CommentMapper
{
    public static Comment ToCommentFromCreate(this CreateCommentRequestDTO commentDto, string userId)
    {
        return new Comment
        {
            Content = commentDto.Content,
            TaskId = commentDto.TaskId,
            UserId = userId
        };
    }

    public static CommentDTO toCommentDto(this Comment commentModel)
    {
        return new CommentDTO
        {
            CommentId = commentModel.CommentId,
            Content = commentModel.Content,
            DateAdded = commentModel.DateAdded,
            UserId = commentModel.UserId,
            TaskId = commentModel.TaskId
        };
    }
}