using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace backend.DTOs.User;

public class EditUserDTO
{
    [Required] public string? Id { get; set; }
    [Required] public string? FirstName { get; set; }
    [Required] public string? LastName { get; set; }
    [Required] [EmailAddress] public string? Email { get; set; }
    [Required] public string? Password { get; set; }
    [Required] public string? Role { get; set; }
    
}