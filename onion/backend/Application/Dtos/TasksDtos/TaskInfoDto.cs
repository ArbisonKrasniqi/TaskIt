namespace Application.Dtos.TasksDtos;

public class TaskInfoDto
{
    public int TaskId { get; set; }
    public string TaskTitle { get; set; }
    public string ListTitle { get; set; }
    public string BoardTitle { get; set; }
    public DateTime DueDate { get; set; }

    public TaskInfoDto(int TaskId, string TaskTitle, string ListTitle, string BoardTitle, DateTime DueDate)
    {
        this.TaskId = TaskId;
        this.TaskTitle = TaskTitle;
        this.ListTitle = ListTitle;
        this.BoardTitle = BoardTitle;
        this.DueDate = DueDate;
    }
}