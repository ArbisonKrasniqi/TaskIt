using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Workspace;

public class OwnerIdDTO
{
 [Required]
 public string? OwnerId { get; set; }
}