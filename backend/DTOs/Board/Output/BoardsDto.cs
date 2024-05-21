namespace backend.DTOs.Board.Output;
using System.ComponentModel.DataAnnotations;

    public class BoardsDto
    {
        public int BoardId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;
    }
