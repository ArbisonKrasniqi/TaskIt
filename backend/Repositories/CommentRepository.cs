using backend.Data;
using backend.DTOs.Comment;
using backend.DTOs.Comment.Output;
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

    public async Task<List<CommentDTO>> GetAllCommentsAsync()
    {
        var comments = await _context.Comment
            .Join(_context.Users,
                c => c.UserId,
                u => u.Id,
                (c, u) => new CommentDTO
                {
                    CommentId = c.CommentId,
                    TaskId = c.TaskId,
                    UserId = c.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Content = c.Content,
                    DateAdded = c.DateAdded
                })
            .ToListAsync();

        return comments;
    }

    public async Task<CommentDTO> GetCommentByIdAsync(int commentId)
    {
        var comment = await _context.Comment
            .Where(c => c.CommentId == commentId)
            .Join(_context.Users,
                c => c.UserId,
                u => u.Id,
                (c, u) => new CommentDTO
                {
                    CommentId = c.CommentId,
                    TaskId = c.TaskId,
                    UserId = c.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Content = c.Content,
                    DateAdded = c.DateAdded
                })
            .FirstOrDefaultAsync();

        return comment;
    }

    public async Task<List<CommentDTO>> GetCommentsByTaskIdAsync(int taskId)
    {
        var taskModel = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
        if (taskModel == null) return null;

        var comments = await _context.Comment
            .Where(c => c.TaskId == taskId)
            .Join(_context.Users,
                c => c.UserId,
                u => u.Id,
                (c, u) => new CommentDTO
                {
                    CommentId = c.CommentId,
                    TaskId = c.TaskId,
                    UserId = c.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Content = c.Content,
                    DateAdded = c.DateAdded
                })
            .ToListAsync();

        return comments;
    }

    public async Task<List<CommentDTO>> GetCommentsByUserIdAdminAsync(string userId)
    {
        var comments = await _context.Comment
            .Where(c => c.UserId == userId)
            .Join(_context.Users,
                c => c.UserId,
                u => u.Id,
                (c, u) => new CommentDTO
                {
                    CommentId = c.CommentId,
                    TaskId = c.TaskId,
                    UserId = c.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Content = c.Content,
                    DateAdded = c.DateAdded
                })
            .ToListAsync();

        return comments;
    }
    
    public async Task<List<CommentDTO>> GetCommentsByUserIdAsync(string userId)
    {
        var comments = await _context.Comment
        .Where(c => c.UserId == userId)
        .Join(_context.Tasks,
            c => c.TaskId,
            t => t.TaskId,
            (c, t) => new { Comment = c, Task = t })
        .Join(_context.List,
            ct => ct.Task.ListId,
            l => l.ListId,
            (ct, l) => new { ct.Comment, ct.Task, List = l })
        .Join(_context.Board,
            ctl => ctl.List.BoardId,
            b => b.BoardId,
            (ctl, b) => new { ctl.Comment, ctl.Task, ctl.List, Board = b })
        .Where(ctlb => !ctlb.Board.IsClosed)
        .Join(_context.Users, // Join with the Users table
            ctlb => ctlb.Comment.UserId,
            u => u.Id,
            (ctlb, u) => new { ctlb.Comment, ctlb.Task, ctlb.List, ctlb.Board, User = u })
        .Select(ctlbu => new CommentDTO
        {
            CommentId = ctlbu.Comment.CommentId,
            TaskId = ctlbu.Comment.TaskId,
            UserId = ctlbu.Comment.UserId,
            FirstName = ctlbu.User.FirstName,  // Use ctlbu.User for user details
            LastName = ctlbu.User.LastName,
            Content = ctlbu.Comment.Content,
            DateAdded = ctlbu.Comment.DateAdded
        })
        .ToListAsync();

        return comments;

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