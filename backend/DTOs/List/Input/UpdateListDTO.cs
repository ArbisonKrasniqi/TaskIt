﻿using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.List;

public class UpdateListDTO
{
    public int ListId { get; set; }
    
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
}