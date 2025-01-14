using Application.Dtos.StarredBoardDtos;
using Application.Handlers.StarredBoard;
using Application.Services.Authorization;
using Domain.Interfaces;

namespace Application.Services.StarredBoard;

public class StarredBoardService : IStarredBoardService
{
    private readonly IStarredBoardRepository _starredBoardRepository;
    private readonly IStarredBoardDeleteHandler _starredBoardDeleteHandler;
    private readonly UserContext _userContext;
    private readonly IAuthorizationService _authorizationService;

    public StarredBoardService(IStarredBoardRepository starredBoardRepository, IStarredBoardDeleteHandler starredBoardDeleteHandler, UserContext userContext, IAuthorizationService authorizationService)
    {
        _starredBoardRepository = starredBoardRepository;
        _starredBoardDeleteHandler = starredBoardDeleteHandler;
        _userContext = userContext;
        _authorizationService = authorizationService;
    }
    public async Task<List<StarredBoardDto>> GetStarredBoardsByUserId(string userId)
    {
        if (userId != _userContext.Id && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var starredBoards = await _starredBoardRepository.GetStarredBoards(userId: userId);
        var starredBoardsDto = new List<StarredBoardDto>();
        foreach (var starredBoard in starredBoards)
        {
            starredBoardsDto.Add(new StarredBoardDto(starredBoard));
        }

        return starredBoardsDto;
    }

    public async Task<List<StarredBoardDto>> GetStarredBoardsByWorkspaceId(int workspaceId)
    {
        var isMember = await _authorizationService.IsMember(_userContext.Id, workspaceId);
        var isOwner = await _authorizationService.OwnsWorkspace(_userContext.Id, workspaceId);
        if (!isMember && !isOwner && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var starredBoards = await _starredBoardRepository.GetStarredBoards(workspaceId: workspaceId);
        var starredBoardsDto = new List<StarredBoardDto>();
        foreach (var starredBoard in starredBoards)
        {
            starredBoardsDto.Add(new StarredBoardDto(starredBoard));
        }

        return starredBoardsDto;
    }

    public async Task<StarredBoardDto> StarBoard(CreateStarredBoardDto createStarredBoardDto)
    {
        var isMember = await _authorizationService.IsMember(_userContext.Id, createStarredBoardDto.WorkspaceId);
        if (!await _authorizationService.CanAccessBoard(_userContext.Id, createStarredBoardDto.BoardId) && !isMember)
            throw new Exception("You are not authorized");

        var alreadyStarredBoard =
            await _starredBoardRepository.GetStarredBoards(boardId: createStarredBoardDto.BoardId);
        if (alreadyStarredBoard.Any())
            throw new Exception("Board is already starred");
        
        var newStarredBoard = new Domain.Entities.StarredBoard(
            createStarredBoardDto.BoardId,
            createStarredBoardDto.UserId,
            createStarredBoardDto.WorkspaceId);

        var addedStarredBoard = await _starredBoardRepository.CreateStarredBoard(newStarredBoard);
        return new StarredBoardDto(addedStarredBoard);
    }

    public async Task<StarredBoardDto> UnstarBoard(StarredBoardIdDto starredBoardIdDto)
    {
        var boards = await _starredBoardRepository.GetStarredBoards(starredBoardId: starredBoardIdDto.StarredBoardId);
        var board = boards.FirstOrDefault();
        
        if (board.UserId != _userContext.Id && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var starredBoards = await _starredBoardRepository.GetStarredBoards(starredBoardId: starredBoardIdDto.StarredBoardId);
        var starredBoard = starredBoards.FirstOrDefault();
        if (starredBoard == null)
            throw new Exception("StarredBoard not found");

        await _starredBoardDeleteHandler.HandleDeleteRequest(starredBoard.StarredBoardId);

        return new StarredBoardDto(starredBoard);
    }
}