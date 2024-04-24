using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Board.Input;

public class BoardIdDto
{
    [Required] 
    [Range(0, int.MaxValue, ErrorMessage = "BoardId Can Not Be Negative!")]
    public int BoardId{ get; set; }
}