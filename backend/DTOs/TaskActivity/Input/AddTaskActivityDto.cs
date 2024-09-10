namespace backend.DTOs.TaskActivity.Input;

public class AddTaskActivityDto
{
    public int TaskId { get; set; }
    public string ActionType { get; set; }
    public string EntityName { get; set; }
}