using backend.DTOs.Board.Input;
using backend.Models;

namespace backend.Interfaces
{
    public interface IBoardRepository
    {
        Task<List<Board>> GetAllBoardsAsync();
        Task<Board?> GetBoardByIdAsync(int id);
        Task<List<Board>?> GetBoardsByWorkspaceIdAsync(int workspaceId);
        Task<Board> CreateBoardAsync(Board board);
        Task<Board?> UpdateBoardAsync(UpdateBoardRequestDto boardDto);
        Task<Board?> DeleteBoardAsync(int id);
        Task<List<Board>> DeleteBoardsByWorkspaceIdAsync(int workspaceId);
        Task<bool> BoardExists(int boardId);
        Task<bool> CloseBoardAsync(int boardId, string userId);
        Task<bool> ReopenBoardAsync(int boardId, string userId);
        Task<List<Board>> GetClosedBoardsAsync(int workspaceId);
        Task<List<Board>> GetUnclosedBoardsAsync(int workspaceId);
        Task<bool> BoardInWorkspace(int boardId, int workspaceId);
    }
}