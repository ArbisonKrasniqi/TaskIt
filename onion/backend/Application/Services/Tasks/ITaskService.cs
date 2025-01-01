using Application.Dtos.TasksDtos;

namespace Application.Services.Tasks;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllTasks();
    Task<TaskDto> GetTaskById(int taskId);
    Task<TaskDto> GetTaskByWorkspaceId(int workspaceId);
    Task<TaskDto> GetTasksByBoardId(int boardId);
    Task<TaskDto> getTasksByListId(int listId);
    Task<TaskDto> UpdateTask(UpdateTaskDto updateTaskDto);
    Task<TaskDto> DeleteTask(TaskIdDto taskIdDto);
    Task<TaskDto> CreateTask(CreateTaskDto createTaskDto);
    Task<TaskDto> DragNDropTask(DragNDropTaskDto dragNDropTaskDto);
}