using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos;

public class EditUserPasswordDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Password { get; set; }
}