using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class Tasks{
        [Key]
        public int TaskId { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Index value cannot be negative!")]
        public int index { get; set; } 
        [Required]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(2, ErrorMessage = "Description must be at least 2 characters")]
        [MaxLength(280, ErrorMessage = "Description cannot be over 280 characters")]
        public string Description { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "List Id can not be negative!")]
        public int ListId { get; set;}

        public List<TaskMember> TaskMembers { get; set; }
    }

    
}