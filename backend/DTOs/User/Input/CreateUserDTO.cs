using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class CreateUserDTO
{
    [Required]
    [MinLength(2, ErrorMessage = "First name should be at least 2 characters")]
    [MaxLength(20, ErrorMessage = "First name cannot be over 20 characters")]
    public string? FirstName { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Last name should be at least 2 characters")]
    [MaxLength(20, ErrorMessage = "Last name cannot be over 20 characters")]
    public string? LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }

    [Required]
    public bool IsAdmin { get; set; }
}