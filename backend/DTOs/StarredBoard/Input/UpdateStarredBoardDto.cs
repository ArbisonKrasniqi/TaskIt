namespace backend.DTOs.StarredBoard;

public class UpdateStarredBoardDto
{
    public int StarredBoardId { get; set; }
    public int BoardId { get; set; }
    public string UserId { get; set; }
}