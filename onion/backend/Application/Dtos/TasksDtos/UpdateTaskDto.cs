using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.TasksDtos;

public class UpdateTaskDto
{
    [Required]
    public int TaskId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
}