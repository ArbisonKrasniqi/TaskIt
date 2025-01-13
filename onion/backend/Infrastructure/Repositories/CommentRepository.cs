using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetComments(int? commentId = null, int? taskId = null, string userId = null,int? listId = null, int? boardId = null,int? workspaceId = null)
    {
        var query = _context.Comments.AsQueryable();

        query = query.Include(c => c.Task)
            .ThenInclude(t => t.Comments)
            .Include(c => c.Task.List)
            .ThenInclude(l => l.Board)
            .ThenInclude(w => w.Workspace);
        
        if (commentId.HasValue)
            query = query.Where(c => c.CommentId == commentId);
        if (taskId.HasValue)
            query = query.Where(c => c.TaskId == taskId);
        if (!string.IsNullOrEmpty(userId))
            query = query.Where(c => c.UserId == userId);
        if(listId.HasValue)
            query = query.Where(c => c.Task.ListId == listId);
        if (boardId.HasValue)
            query = query.Where(c => c.Task.List.BoardId == boardId);
        if (workspaceId.HasValue)
            query = query.Where(c => c.Task.List.Board.WorkspaceId == workspaceId);

        return await query.ToListAsync();

    }

    public async Task<Comment> CreateComment(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment> UpdateComment(Comment comment)
    {
        var existingComment = await _context.Comments.FindAsync(comment.CommentId);
        if (existingComment == null)
        {
            throw new Exception("Comment not found");
        }
        
        _context.Entry(existingComment).CurrentValues.SetValues(comment);
        await _context.SaveChangesAsync();
        return existingComment;
    }

    public async Task<Comment> DeleteComment(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return comment;
    }
    
}
