using Application.Dtos.StarredBoardDtos;
using Domain.Interfaces;

namespace Application.Services.StarredBoard;

public class StarredBoardService : IStarredBoardService
{
    private readonly IUserRepository _userRepository;
    //private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IStarredBoardRepository _starredBoardRepository;
    //private readonly IMembersRepository _membersRepository;
    
    public async Task<List<StarredBoardDto>> GetStarredBoardsByUserId(string userId)
    {
        var starredBoards = await _starredBoardRepository.GetStarredBoards(userId: userId);
        var starredBoardsDto = new List<StarredBoardDto>();
        foreach (var starredBoard in starredBoards)
        {
            starredBoardsDto.Add(new StarredBoardDto(starredBoard));
        }

        return starredBoardsDto;
    }

    public async Task<List<StarredBoardDto>> GetStarredBoardsByWorkspaceId(int workspaceid)
    {
        var starredBoards = await _starredBoardRepository.GetStarredBoards(workspaceId: workspaceid);
        var starredBoardsDto = new List<StarredBoardDto>();
        foreach (var starredBoard in starredBoards)
        {
            starredBoardsDto.Add(new StarredBoardDto(starredBoard));
        }

        return starredBoardsDto;
    }

    public async Task<StarredBoardDto> StarBoard(CreateStarredBoardDto createStarredBoardDto)
    {
        var newStarredBoard = new Domain.Entities.StarredBoard(
            createStarredBoardDto.BoardId,
            createStarredBoardDto.UserId,
            createStarredBoardDto.WorkspaceId);

        var addedStarredBoard = await _starredBoardRepository.CreateStarredBoard(newStarredBoard);
        return new StarredBoardDto(addedStarredBoard);
    }

    public async Task<StarredBoardDto> UnstarBoard(StarredBoardIdDto starredBoardIdDto)
    {
        var starredBoards = await _starredBoardRepository.GetStarredBoards(starredBoardId: starredBoardIdDto.StarredBoardId);
        var starredBoard = starredBoards.FirstOrDefault();
        if (starredBoard == null)
            throw new Exception("StarredBoard not found");

        await _starredBoardRepository.DeleteStarredBoard(starredBoard.StarredBoardId);

        return new StarredBoardDto(starredBoard);
    }
}