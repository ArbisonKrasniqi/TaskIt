using Domain.Interfaces;

namespace Application.Handlers.Comment;

public class CommentDeleteHandler : ICommentDeleteHandler
{
    private readonly ICommentRepository _commentRepository;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepo;
    private readonly UserContext _userContext;

    public CommentDeleteHandler(ICommentRepository commentRepository, IWorkspaceActivityRepository workspaceActivityRepo, UserContext userContext)
    {
        _commentRepository = commentRepository;
        _workspaceActivityRepo = workspaceActivityRepo;
        _userContext = userContext;
    }

    public async Task HandleDeleteRequest(int commentId)
    {
        var comment = (await _commentRepository.GetComments(commentId: commentId)).FirstOrDefault();
        await _commentRepository.DeleteComment(commentId);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(comment.Task.List.Board.Workspace.WorkspaceId,
            _userContext.Id,
            "Deleted",
            comment.Content,
            DateTime.Now);
        await _workspaceActivityRepo.CreateWorkspaceActivity(newActivity);
    }
}