using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Board.Input
{
    public class UpdateBoardRequestDto
    {
        public int BoardId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "BackgroundId Can Not Be Negative!")]
        public int BackgroundId { get; set;}
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "WorkspaceId Can Not Be Negative!")]
        public bool? IsClosed { get; set; }


    }
}