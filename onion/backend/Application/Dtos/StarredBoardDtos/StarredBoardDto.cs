using Domain.Entities;

namespace Application.Dtos.StarredBoardDtos;

public class StarredBoardDto
{
    public int StarredBoardId { get; set; }
    public int BoardId { get; set; }
    public string UserId { get; set; }
    public int WorkspaceId { get; set; }

    public StarredBoardDto(StarredBoard starredBoard)
    {
        StarredBoardId = starredBoard.StarredBoardId;
        BoardId = starredBoard.BoardId;
        UserId = starredBoard.UserId;
        WorkspaceId = starredBoard.WorkspaceId;
    }
}