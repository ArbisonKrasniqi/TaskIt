using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class LoginDTO
//This DTO is used when a user sends data to the backend to log in
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}