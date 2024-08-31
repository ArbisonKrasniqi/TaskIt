using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class TaskMember
{
    [Key]
    public int TaskMemberId { get; set; }
    [Required]
    public string UserId { get; set; } = string.Empty;

    public User User { get; set; }
    [Required]
    public DateTime DateJoined { get; set; }
    [Required]
    public int  TaskId { get; set; }
}