namespace backend.Models;

public class BoardActivity{

    public int BoardActivityId { get; set;}
    public int BoardId { get; set; }
    public Board Board { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string ActionType { get; set; } 
    public string EntityName { get; set; } 
    public DateTime ActionDate { get; set; }
}