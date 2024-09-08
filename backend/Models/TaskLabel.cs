using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class TaskLabel
{
    [Key]
    public int TaskLabelId { get; set; }
    public int TaskId { get; set; }
    public int LabelId { get; set; }
    public DateTime DateAssigned { get; set; } = DateTime.Now;
    public virtual Tasks Task { get; set; }
    public virtual Label Label { get; set; }
}