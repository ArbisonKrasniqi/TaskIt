using backend.Data;
using backend.DTOs.Comment;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext _context;

    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateCommentAsync(Comment commentModel)
    {
        commentModel.DateAdded = DateTime.Now;
        await _context.Comment.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        return await _context.Comment.ToListAsync();
    }

    public async Task<Comment> GetCommentByIdAsync(int commentId)
    {
        return await _context.Comment.FirstOrDefaultAsync(c => c.CommentId == commentId);
    }

    public async Task<List<Comment>> GetCommentsByTaskIdAsync(int taskId)
    {
        var taskModel = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
        if (taskModel == null) return null;

        return await _context.Comment.Where(c => c.TaskId == taskId).ToListAsync();
    }

    public async Task<List<Comment>> GetCommentsByUserIdAsync(string userId)
    {
        return await _context.Comment.Where(c => c.UserId == userId).ToListAsync();
    }

    public async Task<Comment?> DeleteCommentAsync(int commentId)
    {
        var commentModel = await _context.Comment.FirstOrDefaultAsync(c => c.CommentId == commentId);
        if (commentModel == null)
        {
            return null;
        }

        _context.Comment.Remove(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment> UpdateCommentAsync(UpdateCommentDTO updateCommentDto)
    {
        var existingComment =
            await _context.Comment.FirstOrDefaultAsync(c => c.CommentId == updateCommentDto.CommentId);
        if (existingComment == null) return null;

        existingComment.Content = updateCommentDto.Content;

        await _context.SaveChangesAsync();
        return existingComment;
    }
}