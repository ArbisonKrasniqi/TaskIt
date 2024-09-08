using System.ComponentModel.DataAnnotations;


namespace backend.DTOs.Label;

public class UpdateLabelRequestDTO
{
    public int LabelId { get; set; }
    
    [MaxLength(280, ErrorMessage = "Name cannot be over 280 characters")]
    public string Name { get; set; } = string.Empty;
}