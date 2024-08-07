using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        [MaxLength(20, ErrorMessage = "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int? BackgroundId { get; set; }
        public int? WorkspaceId { get; set; }
        public List<List>? Lists { get; set; }
        public bool Starred { get; set; } = false;
        public bool IsClosed { get; set; } = false;
    }
}