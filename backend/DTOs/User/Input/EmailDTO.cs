using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class EmailDTO
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}