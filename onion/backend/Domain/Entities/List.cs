using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class List
{
    [Key]
    public int ListId { get; set; }
    public string Title { get; set; } 
    public int Index { get; set; }
    public DateTime DateCreated { get; set; } 
    public int BoardId { get; set; }
    
    //public List<Tasks> Tasks { get; set; } = new List<Tasks>();
    
    public List(){}
}