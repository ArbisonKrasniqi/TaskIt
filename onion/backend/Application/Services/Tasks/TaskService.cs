using Application.Dtos.TasksDtos;

namespace Application.Services.Tasks;

public class TaskService : ITaskService
{
    public Task<List<TaskDto>> GetAllTasks()
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> GetTaskById(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> GetTaskByWorkspaceId(int workspaceId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> GetTasksByBoardId(int boardId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> getTasksByListId(int listId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> UpdateTask(UpdateTaskDto updateTaskDto)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> DeleteTask(TaskIdDto taskIdDto)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> CreateTask(CreateTaskDto createTaskDto)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDto> DragNDropTask(DragNDropTaskDto dragNDropTaskDto)
    {
        throw new NotImplementedException();
    }
}