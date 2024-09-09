using backend.Data;
using backend.DTOs.StarredBoard;
using backend.Interfaces;
 using backend.Models;
 using Microsoft.EntityFrameworkCore;
 
 namespace backend.Repositories;
 
 public class StarredBoardRepository : IStarredBoardRepository
 {
     private readonly ApplicationDBContext _context;
     private readonly IUserRepository _userRepo;

 
 
     public StarredBoardRepository(ApplicationDBContext context, IUserRepository userRepo)
     {
         _context = context;
         _userRepo = userRepo;
       
     }
 
     //STAR BOARD
     public async Task<StarredBoard?> StarBoardAsync(string userId, int boardId, int workspaceId)
     {

         if (!await _userRepo.UserExists(userId))
         {
             throw new Exception("User not found!");
         }
       
         if (await IsBoardStarredAsync(userId, boardId))
         {
             throw new ("This Board has already been starred by this user!");
         }

         //shtimi i testimit se a eshte qaj user member i workspace-it te bordit perkates

         var starredBoard = new StarredBoard
         {
             BoardId = boardId,
             UserId = userId,
             WorkspaceId = workspaceId
         };
         
         await _context.StarredBoard.AddAsync(starredBoard);
         await _context.SaveChangesAsync();
 
         return starredBoard;
     }
 
     //UNSTAR BOARD
     public async Task<StarredBoard?> UnStarBoardAsync(string userId, int boardId, int workspaceId)
     {
         
         if (!await _userRepo.UserExists(userId))
         {
             throw new Exception("User not found!");
         }
         if (!(await IsBoardStarredAsync(userId, boardId)))
         {
             throw new Exception("This Board hasn't been starred before!");
         }

         var starredBoard = await _context.StarredBoard.FirstOrDefaultAsync(x => x.BoardId == boardId && x.UserId == userId && x.WorkspaceId==workspaceId);
         if (starredBoard == null)
         {
             return null;
         }
   
         _context.StarredBoard.Remove(starredBoard);
         await _context.SaveChangesAsync();
         return starredBoard;
     }
 
     //GET STARRED BOARDS
     public async Task<List<StarredBoard>> GetStarredBoardsAsync(string userId)
     {
         return await _context.StarredBoard.Where(x => x.UserId == userId).ToListAsync();
     }
 
     // GET STARRED BOARDS BY WORKSPACE
     public async Task<List<StarredBoard>> GetStarredBoardsByWorkspaceAsync(string userId, int workspaceId)
     {
         return await _context.StarredBoard.Where(x => x.UserId == userId && x.WorkspaceId == workspaceId)
             .ToListAsync();
     }

     //GETSTARRED BOARD BY ID
     public async Task<StarredBoard?> GetStarredBoardByIdAsync(int id)
     {
         return await _context.StarredBoard.FindAsync(id);
     }

     //IS BOARD STARRED
     public async Task<bool> IsBoardStarredAsync(string userId, int boardId)
     {
         return await _context.StarredBoard.AnyAsync(x => x.BoardId == boardId && x.UserId == userId);
     }
     
    //DELETE STARRED BOARD BY BOARD ID
    public async Task<StarredBoard?> DeleteStarredBoardByBoardIdAsync(int id)
    {
        var starredModel = _context.StarredBoard.FirstOrDefault(b => b.BoardId == id);
        if (starredModel == null) return null;
        _context.StarredBoard.Remove(starredModel);
        await _context.SaveChangesAsync();
        return starredModel;

    }
    
    //DELETE STARRED BOARDS BY WORKSPACEID
    public async Task<List<StarredBoard>> DeleteStarredBoardsByWorkspaceIdAsync(int workspaceId)
    {
        var starredBoards = await _context.StarredBoard.Where(x => x.WorkspaceId == workspaceId).ToListAsync();
        _context.StarredBoard.RemoveRange(starredBoards);
        await _context.SaveChangesAsync();
        return starredBoards;
    }
    
//DELETE STARRED BOARD BY ID
    public async Task<StarredBoard?> DeleteStarredBoardByIdAsync(int id)
    {
        var starredModel = _context.StarredBoard.FirstOrDefault(s => s.StarredBoardId == id);
        if (starredModel == null) return null;

        _context.StarredBoard.Remove(starredModel);
        await _context.SaveChangesAsync();
        return starredModel;
    }

    //GET ALL STARRED BOARDS
    public async Task<List<StarredBoard?>> GetAllStarredBoardsAsync()
    {
        return await _context.StarredBoard.ToListAsync();
    }

    //UPDATE
    public async Task<StarredBoard?> UpdateStarredBoardAsync(UpdateStarredBoardDto starredBoardDto)
    {
        var existingStarred =
            await _context.StarredBoard.FirstOrDefaultAsync(s => s.StarredBoardId == starredBoardDto.StarredBoardId);
        if (existingStarred == null)
        {
            throw new Exception("Starred board not found!");
        }

        existingStarred.StarredBoardId = starredBoardDto.StarredBoardId;
        existingStarred.BoardId = starredBoardDto.BoardId;
        existingStarred.UserId = starredBoardDto.UserId;

        await _context.SaveChangesAsync();
        return existingStarred;
    }
 }