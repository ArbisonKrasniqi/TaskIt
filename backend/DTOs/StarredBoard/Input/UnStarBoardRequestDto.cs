namespace backend.DTOs.StarredBoard;

public class UnStarBoardRequestDto
{
    public int BoardId { get; set; } = 0;
    public string UserId { get; set; } = string.Empty;
}