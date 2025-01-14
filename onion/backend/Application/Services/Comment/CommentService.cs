using System.Runtime.InteropServices.JavaScript;
using Application.Dtos.CommentDtos;
using Application.Dtos.ListDtos;
using Application.Handlers.Comment;
using Application.Services.Authorization;
using Domain.Interfaces;

namespace Application.Services.Comment;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepo;
    private readonly UserContext _userContext;
    private readonly ICommentDeleteHandler _deleteHandler;
    private readonly IAuthorizationService _authorizationService;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepo;

    public CommentService(ICommentRepository commentRepo,UserContext userContext, ICommentDeleteHandler deleteHandler,IAuthorizationService authorizationService, IWorkspaceActivityRepository workspaceActivityRepo)
    {
        _commentRepo = commentRepo;
        _userContext = userContext;
        _deleteHandler = deleteHandler;
        _authorizationService = authorizationService;
        _workspaceActivityRepo = workspaceActivityRepo;
    }

    public async Task<List<CommentDto>> GetAllComments()
    {
        var comments = await _commentRepo.GetComments();
        var commentsDto = new List<CommentDto>();
        foreach (var comment in comments)
        {
            commentsDto.Add(new CommentDto(comment));
        }

        return commentsDto;
    }

    public async Task<CommentDto> GetCommentById(int commentId)
    {
        if (!await _authorizationService.CanAccessComment(_userContext.Id, commentId))
            throw new Exception("You are not authorized");
        
        var comment = (await _commentRepo.GetComments(commentId: commentId)).FirstOrDefault();
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        return new CommentDto(comment);
    }

    public async Task<List<CommentDto>> GetCommentByTaskId(int taskId)
    {
        var accessTask = await _authorizationService.CanAccessTask(_userContext.Id, taskId);
        if (!accessTask  && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var comments = await _commentRepo.GetComments(taskId: taskId);
        var commentDtos = new List<CommentDto>();
        foreach (var comment in comments)
        {
            commentDtos.Add(new CommentDto(comment));            
        }
        return commentDtos;
    }

    public async Task<List<CommentDto>> GetCommentByUserId(string userId)
    {
        
        
        var comments = await _commentRepo.GetComments(userId: userId);
        var commentDtos = new List<CommentDto>();
        foreach (var comment in comments)
        {
            commentDtos.Add(new CommentDto(comment));            
        }
        return commentDtos;
    }

    public async Task<CommentDto> CreateComment(CreateCommentDto createCommentDto)
    {
        var accessTask = await _authorizationService.CanAccessTask(_userContext.Id, createCommentDto.TaskId);
        if (!accessTask && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var comment = new Domain.Entities.Comment(
            createCommentDto.Content,
            createCommentDto.TaskId,
            _userContext.Id,
            DateTime.Now
        );
        var newComment = await _commentRepo.CreateComment(comment);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(newComment.Task.List.Board.Workspace.WorkspaceId,
            _userContext.Id,
            "Created",
            newComment.Content,
            DateTime.Now);
        await _workspaceActivityRepo.CreateWorkspaceActivity(newActivity);
        
        return new CommentDto(newComment);
    }

    public async Task<CommentDto> DeleteComment(CommentIdDto commentIdDto)
    {
        var accessComment = await _authorizationService.CanAccessComment(_userContext.Id, commentIdDto.CommentId);
        if (!accessComment && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var comment = (await _commentRepo.GetComments(commentId: commentIdDto.CommentId)).FirstOrDefault();
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        await _deleteHandler.HandleDeleteRequest(comment.CommentId);
        return new CommentDto(comment);
    }

    public async Task<CommentDto> UpdateComment(UpdateCommentDto updateCommentDto)
    {
        var accessComment = await _authorizationService.CanAccessComment(_userContext.Id, updateCommentDto.CommentId);
        if (!accessComment && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var comment = (await _commentRepo.GetComments(commentId: updateCommentDto.CommentId)).FirstOrDefault();
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        comment.CommentId = updateCommentDto.CommentId;
        comment.Content = updateCommentDto.Content;

        var updatedComment = await _commentRepo.UpdateComment(comment);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(updatedComment.Task.List.Board.Workspace.WorkspaceId,
            _userContext.Id,
            "Updated",
            updatedComment.Content,
            DateTime.Now);
        await _workspaceActivityRepo.CreateWorkspaceActivity(newActivity);
        
        
        return new CommentDto(updatedComment);
    }

}