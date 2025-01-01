using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.TasksDtos;

public class TaskIdDto
{
    [Required]
    public int TaskId { get; set; }
}