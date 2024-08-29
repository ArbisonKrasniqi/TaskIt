using backend.DTOs.Comment;
using backend.Models;

namespace backend.Interfaces;

public interface ICommentRepository
{
    Task<Comment> CreateCommentAsync(Comment commentModel);
    Task<List<Comment>> GetAllCommentsAsync();
    Task<Comment> GetCommentByIdAsync(int commentId);
    Task<List<Comment>> GetCommentsByTaskIdAsync(int taskId);
    Task<List<Comment>> GetCommentsByUserIdAsync(string userId);
    Task<Comment?> DeleteCommentAsync(int commentId);
    Task<Comment> UpdateCommentAsync(UpdateCommentDTO updateCommentDto);
    
}