using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DTOs.TaskMember.Output;

public class TaskMemberDto
{
    public int TaskMemberId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateJoined { get; set; }
    public int  TaskId { get; set; }

}