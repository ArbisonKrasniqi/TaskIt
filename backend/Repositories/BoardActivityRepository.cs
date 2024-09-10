using backend.Data;
using backend.DTOs.BoardActivity.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class BoardActivityRepository : IBoardActivityRepository{
    private readonly ApplicationDBContext _context;

    public BoardActivityRepository(ApplicationDBContext context){
        _context = context;
    
    }

    public async Task<BoardActivity> CreateBoardActivityAsync(BoardActivity boardActivity){

        boardActivity.ActionDate = DateTime.Now;
        await _context.BoardActivity.AddAsync(boardActivity);
        await _context.SaveChangesAsync();
        return boardActivity;
    }



    public async Task<List<BoardActivityDto>> GetAllBoardActivityAsync(){

        var boardActivity = await _context.BoardActivity
            .Join(_context.Users,
               bo => bo.UserId,
               u => u.Id,
               (bo, u) => new BoardActivityDto
               {
                BoardActivityId = bo.BoardActivityId,
                BoardId = bo.BoardId,
                UserId = bo.UserId,
                UserName = u.FirstName,
                UserLastName = u.LastName,
                ActionType = bo.ActionType,
                EntityName = bo.EntityName,
                ActionDate = bo.ActionDate
            }).ToListAsync();

        return boardActivity;
    }

    public async Task<BoardActivityDto> GetBoardActivityByIdAsync(int boardActivityId){
        
        var boardActivity = await _context.BoardActivity
            .Where(bo => bo.BoardActivityId == boardActivityId)
            .Join(_context.Users,
               bo => bo.UserId,
               u => u.Id,
               (bo, u) => new BoardActivityDto
               {
                BoardActivityId = bo.BoardActivityId,
                BoardId = bo.BoardId,
                UserId = bo.UserId,
                UserName = u.FirstName,
                UserLastName = u.LastName,
                ActionType = bo.ActionType,
                EntityName = bo.EntityName,
                ActionDate = bo.ActionDate
               }).FirstOrDefaultAsync();
        return boardActivity;
    }


    public async Task<List<BoardActivityDto>> GetBoardActivityByBoardId(int boardId){

        var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardId == boardId);
        if(board == null){
            return null;
        }

        var boardActivity = await _context.BoardActivity
            .Where(bo => bo.BoardId == boardId)
            .Join(_context.Users,
               bo => bo.UserId,
               u => u.Id,
               (bo, u) => new BoardActivityDto
               {
                BoardActivityId = bo.BoardActivityId,
                BoardId = bo.BoardId,
                UserId = bo.UserId,
                UserName = u.FirstName,
                UserLastName = u.LastName,
                ActionType = bo.ActionType,
                EntityName = bo.EntityName,
                ActionDate = bo.ActionDate
               }).ToListAsync();
        return boardActivity;
        

    }


    public async Task<List<BoardActivityDto>> GetBoardActivityByWorkspaceIdAsync(int workspaceId){
        // i merr krejt boards qe jane ne workspace
        var boards = await _context.Board
            .Where(b => b.WorkspaceId == workspaceId)  // e shikon se a ka board nje workspaceid
            .Select(b => b.BoardId)  // selekton veq boardsid
        .ToListAsync();

        if (boards == null){
            return null;
        }

    
        var boardActivity = await _context.BoardActivity
            .Where(ba => boards.Contains(ba.BoardId))  
            .Join(_context.Users,
               ba => ba.UserId,
               u => u.Id,
               (ba, u) => new BoardActivityDto
               {
                   BoardActivityId = ba.BoardActivityId,
                   BoardId = ba.BoardId,
                   UserId = ba.UserId,
                   UserName = u.FirstName,
                   UserLastName = u.LastName,
                   ActionType = ba.ActionType,
                   EntityName = ba.EntityName,
                   ActionDate = ba.ActionDate
               }).ToListAsync();

        return boardActivity;
    }


    public async Task<BoardActivity?> DeleteBoardActivityByIdAsync(int boardActivityId){
        
        var boardActivity = await _context.BoardActivity.FirstOrDefaultAsync(bo => bo.BoardActivityId == boardActivityId);
        if(boardActivity == null){
            return null;
        }
        _context.BoardActivity.Remove(boardActivity);
        await _context.SaveChangesAsync();
        return boardActivity;
    }


    public async Task<List<BoardActivity?>> DeleteBoardActivityByBoard(int boardId){
         
        var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardId == boardId);
        if(board == null){
            return null;
        }
        var boardActivity = await _context.BoardActivity
            .Where(bo => bo.BoardId == boardId).ToListAsync();
        _context.BoardActivity.RemoveRange(boardActivity);
        await _context.SaveChangesAsync();
        return boardActivity;
    }





    public async Task<List<BoardActivity?>> DeleteBoardActivityByWorkspaceId(int workspaceId)
    {
        var boards = await _context.Board
            .Where(b => b.WorkspaceId == workspaceId) 
            .Select(b => b.BoardId)  
        .ToListAsync();

        if (boards == null){
            return null; 
        }

        var boardActivity = await _context.BoardActivity
            .Where(ba => boards.Contains(ba.BoardId))  
        .ToListAsync();

        if (boardActivity == null){
            return null; 
        }

        _context.BoardActivity.RemoveRange(boardActivity);
        await _context.SaveChangesAsync();
        return boardActivity; 
    }






    

   
}