namespace backend.DTOs.Task;

public class TaskInfoDto
{
    public int TaskId { get; set; }
    public string TaskTitle { get; set; } = string.Empty;
    public string ListTitle { get; set; } = string.Empty;
    public string BoardTitle { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}