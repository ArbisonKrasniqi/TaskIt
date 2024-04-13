using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User;

public class RegisterDTO
//This DTO is used when a normal user creates an account and they provide the needed details
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}