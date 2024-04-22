using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User.Input;

public class EditRoleDTO
{
    [Required] public string? Id { get; set; }
    [Required] public bool isAdmin  { get; set; }
}