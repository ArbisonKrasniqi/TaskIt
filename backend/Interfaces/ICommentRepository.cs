using backend.DTOs.Comment;
using backend.DTOs.Comment.Output;
using backend.Models;

namespace backend.Interfaces;

public interface ICommentRepository
{
    Task<Comment> CreateCommentAsync(Comment commentModel);
    Task<List<CommentDTO>> GetAllCommentsAsync();
    Task<CommentDTO> GetCommentByIdAsync(int commentId);
    Task<List<CommentDTO>> GetCommentsByTaskIdAsync(int taskId);
    Task<List<CommentDTO>> GetCommentsByUserIdAsync(string userId);
    Task<List<CommentDTO>> GetCommentsByUserIdAdminAsync(string userId);
    Task<Comment?> DeleteCommentAsync(int commentId);
    Task<Comment> UpdateCommentAsync(UpdateCommentDTO updateCommentDto);
    Task<List<Comment>> DeleteCommentsByTaskIdAsync(int taskId);

}