namespace backend.DTOs.BoardActivity.Output;

public class BoardActivityDto{

    public int BoardActivityId { get; set; }
    public int BoardId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string  UserLastName { get; set; }
    public string ActionType { get; set; }
    public string EntityName { get; set; }
    public DateTime ActionDate { get; set; }
}