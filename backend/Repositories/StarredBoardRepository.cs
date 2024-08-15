using backend.Data;
 using backend.Interfaces;
 using backend.Models;
 using Microsoft.EntityFrameworkCore;
 
 namespace backend.Repositories;
 
 public class StarredBoardRepository : IStarredBoardRepository
 {
     private readonly ApplicationDBContext _context;
     private readonly IUserRepository _userRepo;
     private readonly IBoardRepository _boardRepo;
 
 
     public StarredBoardRepository(ApplicationDBContext context, IUserRepository userRepo, IBoardRepository boardRepo)
     {
         _context = context;
         _userRepo = userRepo;
         _boardRepo = boardRepo;
     }
 
     //STAR BOARD
     public async Task<StarredBoard?> StarBoardAsync(string userId, int boardId)
     {

         if (!await _userRepo.UserExists(userId))
         {
             throw new Exception("User not found!");
         }

         if (!await _boardRepo.BoardExists(boardId))
         {
             throw new Exception("Board not found!");
         }
         if (await IsBoardStarredAsync(userId, boardId))
         {
             throw new ("This Board has already been starred by this user!");
         }

         //shtimi i testimit se a eshte qaj user member i workspace-it te bordit perkates

         var starredBoard = new StarredBoard
         {
             BoardId = boardId,
             UserId = userId
         };
         
         await _context.StarredBoard.AddAsync(starredBoard);
         await _context.SaveChangesAsync();
 
         return starredBoard;
     }
 
     //UNSTAR BOARD
     public async Task<StarredBoard?> UnStarBoardAsync(string userId, int boardId)
     {
         
         if (!await _userRepo.UserExists(userId))
         {
             throw new Exception("User not found!");
         }

         if (!await _boardRepo.BoardExists(boardId))
         {
             throw new Exception("Board not found!");
         }
         if (!(await IsBoardStarredAsync(userId, boardId)))
         {
             throw new Exception("This Board hasn't been starred before!");
         }

         var starredBoard = await _context.StarredBoard.FirstOrDefaultAsync(x => x.BoardId == boardId && x.UserId == userId);
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
     
     
 
 
 }