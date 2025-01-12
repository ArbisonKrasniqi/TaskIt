using Application.Dtos.BoardDtos;
using Application.Handlers.Board;
using Application.Services.Authorization;
using Domain.Interfaces;

namespace Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardDeleteHandler _boardDeleteHandler;
    private readonly UserContext _userContext;
    private readonly IAuthorizationService _authorizationService;


    public BoardService(IBoardRepository boardRepository, IBoardDeleteHandler boardDeleteHandler, IAuthorizationService authorizationService, UserContext userContext)
    {
        _boardRepository = boardRepository;
        _boardDeleteHandler = boardDeleteHandler;
        _authorizationService = authorizationService;
        _userContext = userContext;
    }
    
    public async Task<List<BoardDto>> GetAllBoards()
    {
        var boards = await _boardRepository.GetBoards();
        var boardsDto = new List<BoardDto>();
        foreach (var board in boards)
        {
            boardsDto.Add(new BoardDto(board));
        }

        return boardsDto;
    }

    public async Task<List<BoardDto>> GetBoardsByWorkspaceId(int workspaceId)
    {
        var isMember = await _authorizationService.IsMember(_userContext.Id, workspaceId);
        var isOwner = await _authorizationService.OwnsWorkspace(_userContext.Id, workspaceId);
        if (!isMember && !isOwner && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var boards = await _boardRepository.GetBoards(workspaceId: workspaceId);
        var boardsDto = new List<BoardDto>();
        foreach (var board in boards)
        {
            boardsDto.Add(new BoardDto(board));
        }

        return boardsDto;
    }

    public async Task<BoardDto> GetBoardById(int boardId)
    {
        if (!await _authorizationService.CanAccessBoard(_userContext.Id, boardId))
            throw new Exception("You are not authorized");
        
        var boards = await _boardRepository.GetBoards(boardId: boardId, isClosed: false);
        var board = boards.FirstOrDefault();
        if (board == null)
            throw new Exception("Board not found");

        return new BoardDto(board);
    }

    public async Task<List<BoardDto>> GetClosedBoards(int workspaceId)
    {
        var isMember = await _authorizationService.IsMember(_userContext.Id, workspaceId);
        var isOwner = await _authorizationService.OwnsWorkspace(_userContext.Id, workspaceId);
        if (!isMember && !isOwner && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var boards = await _boardRepository.GetBoards(workspaceId: workspaceId, isClosed: true);
        var boardsDto = new List<BoardDto>();
        foreach (var board in boards)
        {
            boardsDto.Add(new BoardDto(board));
        }

        return boardsDto;
    }

    public async Task<BoardDto> CreateBoard(CreateBoardDto createBoardDto)
    {
        var accessesWorkspace = await _authorizationService.IsMember(_userContext.Id, createBoardDto.WorkspaceId);
        if (!accessesWorkspace && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var newBoard = new Domain.Entities.Board(
            createBoardDto.Title,
            createBoardDto.WorkspaceId,
            DateTime.Now);

        var addedBoard = await _boardRepository.CreateBoard(newBoard);
        return new BoardDto(addedBoard);

    }

    public async Task<BoardDto> UpdateBoard(UpdateBoardDto updateBoardDto)
    {
        var accessesBoard = await _authorizationService.CanAccessBoard(_userContext.Id, updateBoardDto.BoardId);
        if (!accessesBoard && _userContext.Role != "Admin")
            throw new Exception("You are not authorized ");
        
        var boards = await _boardRepository.GetBoards(boardId: updateBoardDto.BoardId);
        var board = boards.FirstOrDefault();
        if (board == null)
            throw new Exception("Board Not found");
        if (board.IsClosed)
            throw new Exception("Board is closed");

        board.Title = updateBoardDto.Title;

        var updatedBoard = await _boardRepository.UpdateBoard(board);

        return new BoardDto(updatedBoard);
    }

    public async Task<BoardDto> CloseBoard(BoardIdDto boardIdDto)
    {
        var accessesBoard = await _authorizationService.CanAccessBoard(_userContext.Id, boardIdDto.BoardId);
        if (!accessesBoard && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var boards = await _boardRepository.GetBoards(boardId: boardIdDto.BoardId);
        var board = boards.FirstOrDefault();
        if (board == null)
            throw new Exception("Board not found");
        if (board.IsClosed)
            throw new Exception("Board is closed");

        board.IsClosed = true;

        var closedBoard = await _boardRepository.UpdateBoard(board);

        return new BoardDto(closedBoard);
    }

    public async Task<BoardDto> OpenBoard(BoardIdDto boardIdDto)
    {
        var accessesBoard = await _authorizationService.CanAccessBoard(_userContext.Id, boardIdDto.BoardId);
        if (!accessesBoard && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var boards = await _boardRepository.GetBoards(boardId: boardIdDto.BoardId);
        var board = boards.FirstOrDefault();
        if (board == null)
            throw new Exception("Board not found");
        if (!board.IsClosed)
            throw new Exception("Board is opened");

        board.IsClosed = false;

        var openedBoard = await _boardRepository.UpdateBoard(board);

        return new BoardDto(openedBoard);
    }

    public async Task<BoardDto> DeleteBoard(BoardIdDto boardIdDto)
    {
        var accessesBoard = await _authorizationService.CanAccessBoard(_userContext.Id, boardIdDto.BoardId);
        if (!accessesBoard && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var boards = await _boardRepository.GetBoards(boardId: boardIdDto.BoardId);
        var board = boards.FirstOrDefault();
        if (board == null)
            throw new Exception("Board not found");

        await _boardDeleteHandler.HandleDeleteRequest(board.BoardId);

        return new BoardDto(board);
    }
}