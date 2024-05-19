using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class EditUserDTO
{
    [Required]
    public string? Id { get; set; }
    
    [MinLength(2, ErrorMessage = "Last name should be at least 2 characters")]
    [MaxLength(20, ErrorMessage = "Last name cannot be over 20 characters")]
    [Required]
    public string? FirstName { get; set; }
    
    [MinLength(2, ErrorMessage = "Last name should be at least 2 characters")]
    [MaxLength(20, ErrorMessage = "Last name cannot be over 20 characters")]
    [Required]
    public string? LastName { get; set; }
    
    [Required] [EmailAddress] public string? Email { get; set; }
}