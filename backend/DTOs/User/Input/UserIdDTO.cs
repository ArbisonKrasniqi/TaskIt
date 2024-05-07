using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class UserIdDTO
{
    [Required]
    public string id { get; set; }
}