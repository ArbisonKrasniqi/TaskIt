using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos;

public class UserIdDto
{
    [Required]
    public string Id { get; set; }
}