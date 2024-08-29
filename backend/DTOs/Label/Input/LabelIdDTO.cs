using System.ComponentModel.DataAnnotations;


public class LabelIdDTO{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Label Id cannot be negative!")]

    public int LabelId { get; set; }
}