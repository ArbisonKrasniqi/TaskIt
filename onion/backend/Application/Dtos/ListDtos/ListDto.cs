using Domain.Entities;

namespace Application.Dtos.ListDtos;

public class ListDto
{
    public int ListId { get; set; }
    public string Title { get; set; }
    public int Index { get; set; }
    public int BoardId { get; set; }
    public DateTime DateCreated { get; set; }
    
    //public List<TaskDto> Tasks { get; set; }

    public ListDto(List list)
    {
        ListId = list.ListId;
        Title = list.Title;
        Index = list.Index;
        BoardId = list.BoardId;
        DateCreated = list.DateCreated;
    }
    
    
}