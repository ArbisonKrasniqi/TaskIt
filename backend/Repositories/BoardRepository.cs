using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs.Board.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IStarredBoardRepository _starredRepo;
        private readonly IListRepository _listRepo;
        
        public BoardRepository(ApplicationDBContext context, IStarredBoardRepository starredRepo,IListRepository listRepo)
        {
            _context=context;
            _starredRepo = starredRepo;
            _listRepo = listRepo;
        }
        
        //GetAllAsync --> Gets all boards that exists
        public async Task<List<Board>> GetAllBoardsAsync()
        {
            return await _context.Board
                .Include(b => b.Lists).ToListAsync();
        }
        
        //GetByIdAsync --> Gets the board that has the given Id
        public async Task<Board?> GetBoardByIdAsync(int id)
        {
            return await _context.Board.
                Include(b => b.Lists).FirstOrDefaultAsync(i => i.BoardId == id);
        }
        
        //GetBoardsByWorkspaceIdAsync --> Gets all boards that have the given workspaceId
        public async Task<List<Board>> GetBoardsByWorkspaceIdAsync(int workspaceId)
        {
            return await _context.Board.
                Include(b => b.Lists)
                .Where(b => b.WorkspaceId == workspaceId).ToListAsync();
        }
        
        //CreateAsync -->Adds a new board with the given attributes
        public async Task<Board> CreateBoardAsync(Board boardModel)
        {
            int boardCount = await _context.Board.CountAsync(b => b.WorkspaceId == boardModel.WorkspaceId);
            if (boardCount >= 10)
            {
                throw new InvalidOperationException("Cannot create more than 10 boards in the same workspace.");
            }
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

            await _listRepo.DeleteListsByBoardIdAsync(id);
            await _starredRepo.DeleteStarredBoardByIdAsync(id);
            
            _context.Board.Remove(boardModel);
            await _context.SaveChangesAsync();
            return boardModel;
        }
        
        //DeleteBoardsByWorkspaceIdAsync -->Deletes all boards with the given workspaceId
        public async Task<List<Board>> DeleteBoardsByWorkspaceIdAsync(int workspaceId)
        {
            var boards = await _context.Board
                .Include(b=>b.Lists)
                .Where(b => b.WorkspaceId == workspaceId)
                .ToListAsync();

            foreach (var board in boards)
            {     
                await _listRepo.DeleteListsByBoardIdAsync(board.BoardId);
                await _starredRepo.DeleteStarredBoardByIdAsync(board.BoardId);
            
            }
            _context.Board.RemoveRange(boards);
            await _context.SaveChangesAsync();
            return boards;
        }
        
        //BoardsExists --> Checks if that board exists
        public async Task<bool> BoardExists(int boardId)
        {
            return await _context.Board.AnyAsync(i => i.BoardId == boardId);
        }
        
        
        //Close Board
        public async Task<bool> CloseBoardAsync(int boardId, string userId)
        {
            var workspace = await (from b in _context.Board
                join w in _context.Workspace
                    on b.WorkspaceId equals w.WorkspaceId
                where b.BoardId == boardId
                select w).FirstOrDefaultAsync();
            
            if (workspace == null || workspace.OwnerId != userId)
            {
                return false;
            }

            var board = await _context.Board.FindAsync(boardId);   
            if (board == null)
            {
                return false;
            }

            board.IsClosed = true;
            await _context.SaveChangesAsync();
            return true;
        }
        
        //Reopen Board
        public async Task<bool> ReopenBoardAsync(int boardId, string userId)
        {
            var workspace = await (from b in _context.Board
                join w in _context.Workspace
                    on b.WorkspaceId equals w.WorkspaceId
                where b.BoardId == boardId
                select w).FirstOrDefaultAsync();
            
            if (workspace == null || workspace.OwnerId != userId)
            {
                return false;
            }

            var board = await _context.Board.FindAsync(boardId);   
            if (board == null)
            {
                return false;
            }

            board.IsClosed = false;
            await _context.SaveChangesAsync();
            return true;
        }
        
        //GetClosedBoards
        public async Task<List<Board>> GetClosedBoardsAsync(int workspaceId)
        {
            var closedBoards = await (from b in _context.Board
                join w in _context.Workspace
                    on b.WorkspaceId equals w.WorkspaceId
                where b.IsClosed && w.WorkspaceId==workspaceId
                select b).ToListAsync();
            
            
            return closedBoards;
        }

        //Get unclosed boards
        public async Task<List<Board>> GetUnclosedBoardsAsync(int workspaceId)
        {
            var unclosedBoards = await (from b in _context.Board
                join w in _context.Workspace
                    on b.WorkspaceId equals w.WorkspaceId
                where !b.IsClosed && w.WorkspaceId==workspaceId
                select b).ToListAsync();
            return unclosedBoards;
        }

        public async Task<bool> BoardInWorkspace(int boardId, int workspaceId)
        {
            var workspaceModel = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId);
            var boardModel = await _context.Board.FirstOrDefaultAsync(x => x.BoardId == boardId);

            if (workspaceModel != null && boardModel != null)
            {
                if (boardModel.WorkspaceId == workspaceId)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
    
}