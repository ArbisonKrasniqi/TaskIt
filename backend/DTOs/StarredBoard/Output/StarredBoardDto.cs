namespace backend.DTOs.StarredBoard;

public class StarredBoardDto
{
    public int StarredBoardId { get; set; }
    public int BoardId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
}