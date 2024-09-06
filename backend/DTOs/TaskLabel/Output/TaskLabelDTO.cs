namespace backend.DTOs.TaskLabel.Output;

public class TaskLabelDTO
{
    public int TaskLabelId { get; set; }
    public int TaskId { get; set; }
    public int LabelId { get; set; }
    public DateTime DateAssigned { get; set; }
}