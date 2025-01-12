using Application.Dtos.BoardDtos;
using Application.Handlers.Board;
using Domain.Interfaces;

namespace Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardDeleteHandler _boardDeleteHandler;

    public BoardService(IBoardRepository boardRepository, IBoardDeleteHandler boardDeleteHandler)
    {
        _boardRepository = boardRepository;
        _boardDeleteHandler = boardDeleteHandler;
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
        var boards = await _boardRepository.GetBoards(boardId: boardId, isClosed: false);
        var board = boards.FirstOrDefault();
        if (board == null)
            throw new Exception("Board not found");

        return new BoardDto(board);
    }

    public async Task<List<BoardDto>> GetClosedBoards(int workspaceId)
    {
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
        var newBoard = new Domain.Entities.Board(
            createBoardDto.Title,
            createBoardDto.WorkspaceId,
            DateTime.Now);

        var addedBoard = await _boardRepository.CreateBoard(newBoard);
        return new BoardDto(addedBoard);

    }

    public async Task<BoardDto> UpdateBoard(UpdateBoardDto updateBoardDto)
    {
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
        var boards = await _boardRepository.GetBoards(boardId: boardIdDto.BoardId);
        var board = boards.FirstOrDefault();
        if (board == null)
            throw new Exception("Board not found");

        await _boardDeleteHandler.HandleDeleteRequest(board.BoardId);

        return new BoardDto(board);
    }
}