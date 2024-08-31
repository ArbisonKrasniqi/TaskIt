using System.ComponentModel.DataAnnotations;


namespace backend.DTOs.Task;

public class CreateLabelRequestDTO{
        [Required]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [MaxLength(280, ErrorMessage = "Name cannot be over 280 characters")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid color code.")]
        public string? Color { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public int BoardId { get; set;}
         
}