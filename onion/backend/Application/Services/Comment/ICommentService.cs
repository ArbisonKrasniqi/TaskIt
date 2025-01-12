using Application.Dtos.CommentDtos;

namespace Application.Services.Comment;

public interface ICommentService
{
    Task<List<CommentDto>> GetAllComments();
    Task<CommentDto> GetCommentById(int commentId);
    Task<List<CommentDto>> GetCommentByTaskId(int taskId);
    Task<List<CommentDto>> GetCommentByUserId(string userId);
    Task<CommentDto> CreateComment(CreateCommentDto createCommentDto);
    Task<CommentDto> DeleteComment(CommentIdDto commentIdDto);
    Task<CommentDto> UpdateComment(UpdateCommentDto updateCommentDto);

}