﻿using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.List;

public class CreateListDTO
{
    [Required]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]
    public string Title { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Board Id can not be negative!")]
    public int BoardId { get; set; }
}