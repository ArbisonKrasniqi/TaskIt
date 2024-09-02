using System.ComponentModel.DataAnnotations;


namespace backend.Models
{
    public class Background
    {
        public int BackgroundId { get; set; }
        public string? CreatorId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        public byte[] ImageData { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}