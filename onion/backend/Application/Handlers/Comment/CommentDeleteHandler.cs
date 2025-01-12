using Domain.Interfaces;

namespace Application.Handlers.Comment;

public class CommentDeleteHandler : ICommentDeleteHandler
{
    private readonly ICommentRepository _commentRepository;

    public CommentDeleteHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task HandleDeleteRequest(int commentId)
    {
        await _commentRepository.DeleteComment(commentId);
    }
}