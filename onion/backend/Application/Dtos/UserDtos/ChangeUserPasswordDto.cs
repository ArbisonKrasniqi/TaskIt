using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos;

public class ChangeUserPasswordDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string OldPassword { get; set; }
    [Required]
    private string Password { get; set; }
}