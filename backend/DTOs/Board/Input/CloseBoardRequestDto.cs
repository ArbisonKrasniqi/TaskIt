namespace backend.DTOs.Board.Input;

public class CloseBoardRequestDto
{
    public int BoardId { get; set; }
    public string userId { get; set; } = string.Empty;
}