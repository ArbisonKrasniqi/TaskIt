using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _context;

    public BoardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Board>> GetBoards(int? boardId = null, int? workspaceId = null, bool? isClosed = null)
    {
        var query = _context.Boards.AsQueryable();

        if (boardId.HasValue)
            query = query.Where(b => b.BoardId == boardId);
        if (workspaceId.HasValue)
            query = query.Where(b => b.WorkspaceId == workspaceId);
        if (isClosed.HasValue)
            query = query.Where(b => b.IsClosed == isClosed);

        return await query.ToListAsync();
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