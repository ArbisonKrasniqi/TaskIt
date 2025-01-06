using Domain.Entities;

namespace Application.Dtos.BoardDtos;

public class BoardDto
{
    public int BoardId { get; set; }
    public string Title { get; set; }
    public bool IsClosed { get; set; }
    public int WorkspaceId { get; set; }
    public DateTime DateCreated { get; set; }
    //public List<List> Lists { get; set; }

    public BoardDto(Board board)
    {
        BoardId = board.BoardId;
        Title = board.Title;
        IsClosed = board.IsClosed;
        WorkspaceId = board.WorkspaceId;
        DateCreated = board.DateCreated;
    }
}