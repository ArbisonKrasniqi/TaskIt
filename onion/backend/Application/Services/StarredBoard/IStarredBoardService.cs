using Application.Dtos.StarredBoardDtos;

namespace Application.Services.StarredBoard;

public interface IStarredBoardService
{
    Task<List<StarredBoardDto>> GetStarredBoardsByUserId(string userId);
    Task<List<StarredBoardDto>> GetStarredBoardsByWorkspaceId(int workspaceid);
    Task<StarredBoardDto> StarBoard(CreateStarredBoardDto createStarredBoardDto);
    Task<StarredBoardDto> UnstarBoard(StarredBoardIdDto starredBoardIdDto);
}