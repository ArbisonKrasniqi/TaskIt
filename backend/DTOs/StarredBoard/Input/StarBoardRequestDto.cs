namespace backend.DTOs.StarredBoard;

public class StarBoardRequestDto
{
    public int BoardId { get; set; }
    public string UserId { get; set; } = string.Empty;
}