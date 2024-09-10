namespace backend.DTOs.BoardActivity.Input;

public class AddBoardActivityDto{
    public int BoardId { get; set; }
    public string ActionType { get; set; }
    public string EntityName { get; set; }
}