namespace backend.DTOs.TaskMember.Input;

public class UpdateTaskMemberDto
{
    public int TaskMemberId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime DateJoined { get; set; }
    public int  TaskId { get; set; }
}