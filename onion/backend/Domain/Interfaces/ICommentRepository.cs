using Domain.Entities;

namespace Domain.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetComments(
        int? commentId = null,
        int? taskId = null,
        int? userId = null
    );
    Task<Comment> CreateComment(Comment comment);
    Task<Comment> UpdateComment(Comment comment);
    Task<Comment> DeleteComment(int commentId);
}