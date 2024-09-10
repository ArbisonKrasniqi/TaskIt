namespace backend.DTOs.TaskActivity.Output;

public class TaskActivityDto
{
    public int TaskActivityId { get; set; }
    public int TaskId { get; set; }
    public string UserId { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string ActionType { get; set; }
    public string EntityName { get; set; }
    public DateTime ActionDate { get; set; }
}