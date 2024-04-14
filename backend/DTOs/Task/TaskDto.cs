namespace backend.DTOs.Task;

public class TaskDto{
    public int TaskID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ListId { get; set; }
}