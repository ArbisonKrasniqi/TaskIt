using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class EditUserPasswordDTO
{
    [Required]
    public string? Id { get; set; }
    
    [Required]
    public string? Password { get; set; }
}