using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ApplicationDBContext _context;
        public BoardRepository(ApplicationDBContext context)
        {
            _context=context;
        }

        public object Board => throw new NotImplementedException();

        public async Task<Board> CreateAsync(Board boardModel)
        {
            await _context.Boards.AddAsync(boardModel);
            await _context.SaveChangesAsync();
            return boardModel;
        }

        public async Task<Board?> DeleteAsync(int id)
        {
            var boardModel = await _context.Boards.FirstOrDefaultAsync(x => x.BoardId == id);

            if (boardModel == null)
                return null;

            _context.Boards.Remove(boardModel);
            await _context.SaveChangesAsync();
            return boardModel;
        }

        public async Task<List<Board>> DeleteBoardsAsync(int workspaceId)
        {
            var boardsToDelete = await _context.Boards.Where(x => x.WorkspaceId == workspaceId).ToListAsync();

            _context.Boards.RemoveRange(boardsToDelete);
            await _context.SaveChangesAsync();

            return boardsToDelete;
        }

        public async Task<List<Board?>> DeleteBoardsByWorkspaceIdAsync(int workspaceId)
        {
            var boards = await _context.Boards.Where(b => b.WorkspaceId == workspaceId).ToListAsync();
            _context.Boards.RemoveRange(boards);
            await _context.SaveChangesAsync();
            return boards;

        }

        public async Task<List<Board>> GetAllAsync()
        {
            return await _context.Boards.ToListAsync();
        }

        public async Task<List<Board>> GetBoardsByWorkspaceIdAsync(int workspaceId)
        {
            return await _context.Boards.Where(b => b.WorkspaceId == workspaceId).ToListAsync();
        }

        public async Task<Board?> GetByIdAsync(int id)
        {
            return await _context.Boards.FindAsync(id);
        }

        public async Task<Board?> UpdateAsync(int id, Board boardModel)
        {
            var existingBoard = await _context.Boards.FindAsync(id);
            
            if (existingBoard == null)
                return null;

            existingBoard.Title = boardModel.Title;
            existingBoard.BackgroundId = boardModel.BackgroundId;

            await _context.SaveChangesAsync();
            return existingBoard;
        }
    }
}