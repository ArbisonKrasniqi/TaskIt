using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Background.Input;

public class BackgroundIdDto
{
    [Required] 
    [Range(0, int.MaxValue, ErrorMessage = "BackgroundId Can Not Be Negative!")]
    public int BackgroundId{ get; set; }
}