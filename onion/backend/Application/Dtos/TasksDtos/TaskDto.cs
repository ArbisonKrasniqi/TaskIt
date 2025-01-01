using Domain.Entities;

namespace Application.Dtos.TasksDtos;

public class TaskDto
{
    public int TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ListId { get; set; }
    public int Index { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime DateAdded { get; set; }
    //public List<Checklist> Checklists { get; set;}

    public TaskDto(Tasks task)
    {
        TaskId = task.TaskId;
        Title = task.Title;
        Description = task.Description;
        ListId = task.ListId;
        Index = task.Index;
        DueDate = task.DueDate;
        DateAdded = task.DateCreated;
    }
}