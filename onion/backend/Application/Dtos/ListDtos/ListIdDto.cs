using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ListDtos;

public class ListIdDto
{
    [Required]
    public int ListId { get; set; }
}