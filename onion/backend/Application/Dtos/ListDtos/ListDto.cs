using Application.Dtos.TasksDtos;
using Domain.Entities;

namespace Application.Dtos.ListDtos;

public class ListDto
{
    public int ListId { get; set; }
    public string Title { get; set; }
    public int Index { get; set; }
    public int BoardId { get; set; }
    public DateTime DateCreated { get; set; }
    public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();

    public ListDto(List list)
    {
        ListId = list.ListId;
        Title = list.Title;
        Index = list.Index;
        BoardId = list.BoardId;
        DateCreated = list.DateCreated;
        if (list.Tasks != null)
        {
            foreach (var task in list.Tasks)
            {
                Tasks.Add(new TaskDto(task));
            }
        }
    }
    
    
}