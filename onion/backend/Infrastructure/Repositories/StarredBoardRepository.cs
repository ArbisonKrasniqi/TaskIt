using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class StarredBoardRepository : IStarredBoardRepository
{
    private readonly AppDbContext _context;

    public StarredBoardRepository(AppDbContext context)
    {
        _context = context;
    }
    public Task<IEnumerable<StarredBoard>> GetStarredBoards(int? starredBoardId = null, int? boardId = null, string? userId = null, int? workspaceId = null)
    {
        throw new NotImplementedException();
        //Masi kerkon workspaceId duhet me e prit modelin e Workspace per me mujt me i kthy starredBoards edhe sipas workspaceId
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