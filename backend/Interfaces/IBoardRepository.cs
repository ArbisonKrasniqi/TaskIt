using backend.Models;

namespace backend.Interfaces
{
    public interface IBoardRepository
    {
        Task<List<Board>> GetAllBoardsAsync();
        Task<Board?> GetBoardByIdAsync(int id);
        Task<List<Board>?> GetBoardsByWorkspaceIdAsync(int workspaceId);
        Task<Board> CreateBoardAsync(Board board);
        Task<Board?> UpdateBoardAsync(Board boardModel);
        Task<Board?> DeleteBoardAsync(int id);
        Task<List<Board>> DeleteBoardsByWorkspaceIdAsync(int workspaceId);
        Task<bool> BoardExists(int boardId);
    }
}