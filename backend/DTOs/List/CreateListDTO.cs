namespace backend.DTOs.List;

public class CreateListDTO
{
    public string Title { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;
}