using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class ChangePasswordDto
{
    [Required]
    public string? Id { get; set; }
   
    [Required]
    public string? OldPassword { get; set; }
    [Required]
    public string? Password { get; set; }
   
}