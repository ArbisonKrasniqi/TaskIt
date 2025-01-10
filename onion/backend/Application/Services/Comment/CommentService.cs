using Application.Dtos.CommentDtos;
using Application.Dtos.ListDtos;
using Domain.Interfaces;

namespace Application.Services.Comment;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepo;
    private readonly ITasksRepository _tasksRepo;
    private readonly UserContext _userContext;

    public CommentService(ICommentRepository commentRepo, ITasksRepository tasksRepo,UserContext userContext)
    {
        _commentRepo = commentRepo;
        _tasksRepo = tasksRepo;
        _userContext = userContext;
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
        var comments = await _commentRepo.GetComments(commentId: commentId);
        var comment = comments.FirstOrDefault();
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        return new CommentDto(comment);
    }

    public async Task<List<CommentDto>> GetCommentByTaskId(int taskId)
    {
        var comments = await _commentRepo.GetComments(taskId: taskId);
        if (comments == null)
        {
            throw new Exception("Comment not found");
        }

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
        if (comments == null)
        {
            throw new Exception("Comment not found");
        }

        var commentDtos = new List<CommentDto>();
        foreach (var comment in comments)
        {
            commentDtos.Add(new CommentDto(comment));            
        }

        return commentDtos;
    }

    public async Task<CommentDto> CreateComment(CreateCommentDto createCommentDto)
    {
        var newComment = new Domain.Entities.Comment(
            createCommentDto.Content,
            createCommentDto.TaskId
        );

        var addedComment = await _commentRepo.CreateComment(newComment);
        return new CommentDto(newComment);
    }

    public async Task<CommentDto> DeleteComment(CommentIdDto commentIdDto)
    {
        var comments = await _commentRepo.GetComments(commentId: commentIdDto.CommentId);
        var comment = comments.FirstOrDefault();
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        await _commentRepo.DeleteComment(commentIdDto.CommentId);
        return new CommentDto(comment);
    }

    public async Task<CommentDto> UpdateComment(UpdateCommentDto updateCommentDto)
    {
        var comments = await _commentRepo.GetComments(commentId: updateCommentDto.CommentId);
        var comment = comments.FirstOrDefault();
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        comment.CommentId = updateCommentDto.CommentId;
        comment.Content = updateCommentDto.Content;

        var updatedComment = await _commentRepo.UpdateComment(comment);
        return new CommentDto(updatedComment);
    }

}