using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace backend.DTOs.Task;

public class DragNDropTaskDTO
{
    [Required]
    public int TaskId { get; set; }
    
    [Required]
    public int NewIndex { get; set; }
    
    [Required]
    public int ListId { get; set; }
}