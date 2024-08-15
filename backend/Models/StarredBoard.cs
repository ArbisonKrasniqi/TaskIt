namespace backend.Models;

public class StarredBoard
{
    public int StarredBoardId { get; set; }
    public int BoardId { get; set; }
    public string UserId { get; set; }
    public int WorkspaceId { get; set; }
}