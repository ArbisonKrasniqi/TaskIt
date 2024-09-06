namespace backend.DTOs.Label;

public class LabelInfoDto
{
    public int LabelId { get; set;}
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public int BoardId { get; set;}
}