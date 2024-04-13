using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User;

public class CreateUserDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }

    [Required]
    public bool IsAdmin { get; set; }
}