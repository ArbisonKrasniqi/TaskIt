using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Domain.Entities;

public class List
{
    [Key]
    public int ListId { get; set; }
    public string Title { get; set; } 
    public int Index { get; set; }
    public DateTime DateCreated { get; set; } 
    public int BoardId { get; set; }
    
    public Board Board { get; set; } = null;
    public List<Tasks> Tasks { get; set; } = new List<Tasks>();
    
    public List(){}

    public List(string title, int index, DateTime dateCreated, int boardId)
    {
        Title = title;
        Index = index;
        DateCreated = dateCreated;
        BoardId = boardId;
    }

    public List(string title, int boardId)
    {
        Title = title;
        BoardId = boardId;
    }
}