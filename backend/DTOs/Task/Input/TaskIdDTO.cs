using System.ComponentModel.DataAnnotations;

public class TaskIdDTO{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Task Id can not be negative!")]
    public int TaskId { get; set; }
}