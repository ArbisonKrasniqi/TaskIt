using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class TaskMember
{
    [Key]
    public int TaskMemberId { get; set; }
    public int  TaskId { get; set; }
    public Tasks Task { get; set; }
    public string UserId { get; set; } = string.Empty;

    public User User { get; set; }
    [Required]
    public DateTime DateJoined { get; set; }
    
}