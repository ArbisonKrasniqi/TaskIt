using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _context;

    public BoardRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Board>> GetBoards(int? boardId = null, int? workspaceId = null, bool? isClosed = null)
    {
        throw new NotImplementedException();
        //Masi kerkon workspaceId duhet me e prit modelin e Workspace per me mujt me i kthy boards edhe sipas workspaceId
    }

    public async Task<Board> CreateBoard(Board board)
    {
        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();
        return board;
    }

    public async Task<Board> UpdateBoard(Board board)
    {
        var existingBoard = await _context.Boards.FindAsync(board.BoardId);

        if (existingBoard == null)
            throw new Exception("Board not found");
        
        _context.Entry(existingBoard).CurrentValues.SetValues(board);
        await _context.SaveChangesAsync();

        return existingBoard;
    }
    
    public async Task<Board> DeleteBoard(int boardId)
    {
        var board = await _context.Boards.FindAsync(boardId);

        if (board == null)
            throw new Exception("Board not found");

        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();

        return board;
    }
}