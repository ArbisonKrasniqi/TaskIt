using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Tasks
{
    //Properties
    [Key]
    public int TaskId { get; set; }
    public int Index { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DueDate { get; set; }
    public int ListId { get; set; }

    //Navigation
    public List List { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    //Constructors
    public Tasks(){}
    
    public Tasks(int index, string title, DateTime dateCreated, int listId)
    {
        Index = index;
        Title = title;
        DateCreated = dateCreated;
        ListId = listId;
    }
    
    public Tasks(int taskId, int index, string title, string description, DateTime dateCreated, DateTime dueDate, int listId)
    {
        TaskId = taskId;
        Index = index;
        Title = title;
        Description = description;
        DateCreated = dateCreated;
        DueDate = dueDate;
        ListId = listId;
    }
}