using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Checklist.Input;

public class ChecklistIdDTO
{
    [Required]
    [Range(0,int.MaxValue,ErrorMessage = "ChecklistId can not be negative")]
    public int ChecklistId { get; set; }
}