using backend.DTOs.TaskLabelDTO.Input;
using backend.Models;

namespace backend.Interfaces;

public interface ITaskLabelRepository
{
    Task<List<TaskLabel>> getAllTaskLabelsOpenBoardAsync();
    Task<bool> labelIsInOpenBoard(int labelId);
    Task<List<TaskLabel>> getAllTaskLabelsAsync();
    Task<TaskLabel> getTaskLabelByIdAsync(int taskLabelId);
    Task<TaskLabel> getTaskLabelByLabelAndTask(int labelId, int taskId);
    Task<TaskLabel> assignLabelToTask(TaskLabel taskLabelModel);
    Task<bool> TaskLabelExists(AssignLabelDTO assignLabelDto);
    Task<TaskLabel> removeTaskLabel(TaskLabel taskLabelModel);
}