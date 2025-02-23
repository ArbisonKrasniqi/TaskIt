﻿using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Background.Input
{
    public class CreateBackgroundDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}