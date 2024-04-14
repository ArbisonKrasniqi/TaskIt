using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace backend.DTOs.User;

public class EditUserRoleDTO
{
    [Required] public string? Id { get; set; }
    [Required] public bool isAdmin  { get; set; }
}