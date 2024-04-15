namespace backend.Models;

public class List
{
    public int ListId { get; set; }
    public int index { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;
}