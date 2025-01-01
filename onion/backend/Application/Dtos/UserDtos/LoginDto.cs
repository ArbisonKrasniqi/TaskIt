using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}