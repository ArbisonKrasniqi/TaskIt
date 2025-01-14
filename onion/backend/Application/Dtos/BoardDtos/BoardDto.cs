using Application.Dtos.ListDtos;
using Domain.Entities;

namespace Application.Dtos.BoardDtos;

public class BoardDto
{
    public int BoardId { get; set; }
    public string Title { get; set; }
    public bool IsClosed { get; set; }
    public int WorkspaceId { get; set; }
    public DateTime DateCreated { get; set; }
    public List<ListDto> Lists { get; set; } = new List<ListDto>();

    public BoardDto(Board board)
    {
        BoardId = board.BoardId;
        Title = board.Title;
        IsClosed = board.IsClosed;
        WorkspaceId = board.WorkspaceId;
        DateCreated = board.DateCreated;

        if (board.Lists != null)
        {
            foreach (var list in board.Lists)
            {
                Lists.Add(new ListDto(list));
            }
        }
    }
}