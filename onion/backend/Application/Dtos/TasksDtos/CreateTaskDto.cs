using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.TasksDtos;

public class CreateTaskDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public int ListId { get; set; }
}