using backend.DTOs.TaskMember.Output;

namespace backend.DTOs.Task;

public class TaskInfoDto2
{
    public int TaskId { get; set; }
    public string TaskTitle { get; set; } = string.Empty;
    public string ListTitle { get; set; } = string.Empty;
    public string BoardTitle { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public List<Models.Label> Labels { get; set; }
    public List<Models.TaskMember> TaskMembers { get; set; }
}