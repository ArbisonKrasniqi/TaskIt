using Domain.Entities;

namespace Domain.Interfaces;

public interface IStarredBoardRepository
{
    Task<IEnumerable<StarredBoard>> GetStarredBoards(
        int? starredBoardId = null,
        int? boardId = null,
        string? userId = null,
        int? workspaceId = null);

    Task<StarredBoard> CreateStarredBoard(StarredBoard starredBoard);
    Task<StarredBoard> UpdateStarredBoard(StarredBoard starredBoard);
    Task<StarredBoard> DeleteStarredBoard(int starredBoardId);
}