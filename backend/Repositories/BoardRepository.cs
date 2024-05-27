using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs.Board.Input;
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
        
        //GetAllAsync --> Gets all boards that exists
        public async Task<List<Board>> GetAllBoardsAsync()
        {
            return await _context.Board.ToListAsync();
        }
        
        //GetByIdAsync --> Gets the board that has the given Id
        public async Task<Board?> GetBoardByIdAsync(int id)
        {
            return await _context.Board.FirstOrDefaultAsync(i => i.BoardId == id);
        }
        
        //GetBoardsByWorkspaceIdAsync --> Gets all boards that have the given workspaceId
        public async Task<List<Board>> GetBoardsByWorkspaceIdAsync(int workspaceId)
        {
            return await _context.Board.Where(b => b.WorkspaceId == workspaceId).ToListAsync();
        }
        
        //CreateAsync -->Adds a new board with the given attributes
        public async Task<Board> CreateBoardAsync(Board boardModel)
        {
            await _context.Board.AddAsync(boardModel);
            await _context.SaveChangesAsync();
            return boardModel;
        }
        
        //UpdatesAsync -->Gets the board with the given Id and updates with the given attributes
        public async Task<Board?> UpdateBoardAsync(UpdateBoardRequestDto boardDto)
        {
            var existingBoard = await _context.Board.FirstOrDefaultAsync(i => i.BoardId == boardDto.BoardId);
            
            if (existingBoard == null)
                return null;

            existingBoard.Title = boardDto.Title;
            existingBoard.BackgroundId = boardDto.BackgroundId;
            existingBoard.WorkspaceId = boardDto.WorkspaceId;

            await _context.SaveChangesAsync();
            return existingBoard;
        }
        
        //DeleteAsync --> Deletes the board that has the given Id
        public async Task<Board?> DeleteBoardAsync(int id)
        {
            var boardModel = await _context.Board.FirstOrDefaultAsync(x => x.BoardId == id);

            if (boardModel == null)
                return null;

            _context.Board.Remove(boardModel);
            await _context.SaveChangesAsync();
            return boardModel;
        }
        
        //DeleteBoardsByWorkspaceIdAsync -->Deletes all boards with the given workspaceId
        public async Task<List<Board>> DeleteBoardsByWorkspaceIdAsync(int workspaceId)
        {
            var boards = await _context.Board.Where(b => b.WorkspaceId == workspaceId).ToListAsync();

            if (boards.Count == 0) return null;  //If no boards were deleted , return null         

            _context.Board.RemoveRange(boards);
            await _context.SaveChangesAsync();
            return boards;
        }
        
        //BoardsExists --> Checks if that board exists
        public async Task<bool> BoardExists(int boardId)
        {
            return await _context.Board.AnyAsync(i => i.BoardId == boardId);
        }
    }
}