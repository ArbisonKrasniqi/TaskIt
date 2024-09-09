using backend.DTOs.StarredBoard;
using backend.Models;

namespace backend.Interfaces;

public interface IStarredBoardRepository
{
    Task<StarredBoard?> StarBoardAsync(string userId, int boardId,  int workspaceId);
    Task<StarredBoard?> UnStarBoardAsync(string userId, int boardId, int workspaceId);
    Task<List<StarredBoard>> GetStarredBoardsAsync(string userId);
    Task<StarredBoard?> GetStarredBoardByIdAsync(int id);
    Task<bool> IsBoardStarredAsync(string userId, int boardId);
    Task<List<StarredBoard>> GetStarredBoardsByWorkspaceAsync(string userId, int workspaceId);
    Task<StarredBoard?> DeleteStarredBoardByBoardIdAsync(int id);
    Task<StarredBoard?> DeleteStarredBoardByIdAsync(int id);
    Task<List<StarredBoard?>> GetAllStarredBoardsAsync();
    Task<StarredBoard?> UpdateStarredBoardAsync(UpdateStarredBoardDto starredBoardDto);
}