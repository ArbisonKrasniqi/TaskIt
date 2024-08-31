using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.ChecklistItem.Input;

public class ChecklistItemIdDTO
{
    [Required]
    [Range(0,int.MaxValue,ErrorMessage = "CheclistItemId can not be negative")]
    public int ChecklistItemId { get; set; }
}