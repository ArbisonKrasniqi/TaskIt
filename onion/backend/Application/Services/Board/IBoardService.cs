using Application.Dtos.BoardDtos;
using Domain.Entities;

namespace Application.Services;

public interface IBoardService
{
    Task<List<BoardDto>> GetAllBoards();
    Task<List<BoardDto>> GetBoardsByWorkspaceId(int workspaceId);
    Task<BoardDto> GetBoardById(int boardId);
    Task<List<BoardDto>> GetClosedBoards(int workspaceId);
    Task<BoardDto> CreateBoard(CreateBoardDto createBoardDto);
    Task<BoardDto> UpdateBoard(UpdateBoardDto updateBoardDto);
    Task<BoardDto> CloseBoard(BoardIdDto boardIdDto);
    Task<BoardDto> OpenBoard(BoardIdDto boardIdDto);
    Task<BoardDto> DeleteBoard(BoardIdDto boardIdDto);
}