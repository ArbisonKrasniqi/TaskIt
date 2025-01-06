using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Comment>> GetComments(int? commentId = null, int? taskId = null, int? userId = null)
    {
        throw new NotImplementedException();
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
