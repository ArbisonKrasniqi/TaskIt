using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos;

public class EditUserRoleDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public bool IsAdmin { get; set; }
}