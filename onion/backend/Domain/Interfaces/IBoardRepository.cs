using Domain.Entities;

namespace Domain.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetBoards(
        int? boardId = null,
        int? workspaceId = null,
        bool? isClosed = null);

    Task<Board> CreateBoard(Board board);
    Task<Board> UpdateBoard(Board board);
    Task<Board> DeleteBoard(int boardId);
}