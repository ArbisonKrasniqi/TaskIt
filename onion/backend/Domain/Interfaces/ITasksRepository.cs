using Domain.Entities;

namespace Domain.Interfaces;

public interface ITasksRepository
{
    Task<IEnumerable<Tasks>> GetTasks(
        int? taskId = null,
        int? index = null,
        DateTime? dateCreated = null,
        DateTime? dueDate = null,
        int? listId = null,
        int? boardId = null,
        int? workspaceId = null
    );
    Task<Tasks> CreateTask(Tasks task);
    Task<Tasks> UpdateTask(Tasks task);
    Task<Tasks> DeleteTask(int taskId);
}