namespace backend.DTOs.TaskMember.Input;

public class RemoveTaskMemberDto
{
    public string UserId { get; set; }
    public int TaskId { get; set; }
}