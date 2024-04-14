using backend.DTOs.Task;
using backend.Models;

namespace backend.Mappers;

public static class TaskMappers{

    public static TaskDto ToTaskDto (this Task taskModel){
        return new TaskDto{
            TaskID = taskModel.TaskID,
            Title = taskModel.Title,
            Description = taskModel.Description,
            ListId = taskModel.ListId
        };
    }
    public static Task ToTaskFromCreate(this CreateTaskRequestDTO taskDto)
    {
        return new Task
        {
            Title = workspaceDto.Title,
            Description = workspaceDto.Description,
            ListId = workspaceDto.ListId
        };
    }
    public static Task ToTaskFromUpdate(this UpdateTaskRequestDto taskDto)
    {
        return new Task
        {
            Title = workspaceDto.Title,
            Description = workspaceDto.Description,
            ListId = workspaceDto.ListId
        };
    }
}