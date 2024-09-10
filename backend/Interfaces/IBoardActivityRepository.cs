using backend.DTOs.BoardActivity.Output;
using backend.Models;

namespace backend.Interfaces;

public interface IBoardActivityRepository{

    Task<BoardActivity> CreateBoardActivityAsync(BoardActivity boardActivity);
    Task<List<BoardActivityDto>> GetAllBoardActivityAsync();
    Task<BoardActivityDto> GetBoardActivityByIdAsync(int boardActivityId);
    Task<List<BoardActivityDto>> GetBoardActivityByBoardId(int boardId);
    Task<List<BoardActivityDto>> GetBoardActivityByWorkspaceIdAsync (int workspaceId);
    Task<BoardActivity?> DeleteBoardActivityByIdAsync(int boardActivityId);
    Task<List<BoardActivity?>> DeleteBoardActivityByBoard (int boardId);
    Task<List<BoardActivity?>> DeleteBoardActivityByWorkspaceId (int workspaceId);
}