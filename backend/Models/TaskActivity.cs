namespace backend.Models;

public class TaskActivity
{
    public int TaskActivityId { get; set; }
    public int TaskId { get; set; }
    public Tasks Task { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string ActionType { get; set; }
    public string EntityName { get; set; }
    public DateTime ActionDate { get; set; }
}