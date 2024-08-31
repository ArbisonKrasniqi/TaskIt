namespace backend.DTOs.TaskMember.Input;

public class AddTaskMemberDto
{
    public string UserId { get; set; } = string.Empty;
    public int TaskId { get; set; }
}