using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StarredBoardRepository : IStarredBoardRepository
{
    private readonly AppDbContext _context;

    public StarredBoardRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<StarredBoard>> GetStarredBoards(int? starredBoardId = null, int? boardId = null, string? userId = null, int? workspaceId = null)
    {
        var query = _context.StarredBoards.AsQueryable();

        if (starredBoardId.HasValue)
            query = query.Where(s => s.StarredBoardId == starredBoardId);
        if (boardId.HasValue)
            query = query.Where(s => s.BoardId == boardId);
        if (!string.IsNullOrEmpty(userId))
            query = query.Where(s => EF.Functions.Like(s.UserId, $"^{userId}"));

        return await query.ToListAsync();
    }

    public async Task<StarredBoard> CreateStarredBoard(StarredBoard starredBoard)
    {
        await _context.StarredBoards.AddAsync(starredBoard);
        await _context.SaveChangesAsync();
        return starredBoard;
    }

    public async Task<StarredBoard> UpdateStarredBoard(StarredBoard starredBoard)
    {
        var existingStarredBoard = await _context.StarredBoards.FindAsync(starredBoard.StarredBoardId);

        if (existingStarredBoard == null)
            throw new Exception("StarredBoard not found");
        
        _context.Entry(existingStarredBoard).CurrentValues.SetValues(starredBoard);
        await _context.SaveChangesAsync();

        return existingStarredBoard;
    }

    public async Task<StarredBoard> DeleteStarredBoard(int starredBoardId)
    {
        var starredBoard = await _context.StarredBoards.FindAsync(starredBoardId);

        if (starredBoard == null)
            throw new Exception("StarredBoard not found");

        _context.StarredBoards.Remove(starredBoard);
        await _context.SaveChangesAsync();

        return starredBoard;
    }
}