using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class RegisterDTO
//This DTO is used when a normal user creates an account and they provide the needed details
{
    [MinLength(2, ErrorMessage = "Last name should be at least 2 characters")]
    [MaxLength(20, ErrorMessage = "Last name cannot be over 20 characters")]
    [Required]
    public string? FirstName { get; set; }
    
    [MinLength(2, ErrorMessage = "Last name should be at least 2 characters")]
    [MaxLength(20, ErrorMessage = "Last name cannot be over 20 characters")]
    [Required]
    public string? LastName { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}