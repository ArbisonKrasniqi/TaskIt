using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.List;

public class ListIdDTO
{
    [Required] 
    [Range(0, int.MaxValue, ErrorMessage = "List Id can not be negative!")]
    public int ListId{ get; set; }
}