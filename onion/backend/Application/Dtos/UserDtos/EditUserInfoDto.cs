using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos;

public class EditUserInfoDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}