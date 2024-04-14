using backend.DTOs.Task;
using backend.Models;

namespace backend.Mappers;

public static class TaskMappers{

    public static TaskDto ToTaskDto (this Task taskModel){
        return new TaskDto{
            TaskID = taskModel.TaskID,
            Title = taskModel.Title,
            Description = taskModel.Description,
            DateAdded = taskModel.DateAdded,
            DueDate = taskModel.DueDate,
            ListId = taskModel.ListId
        };
    }


    public static Task ToTaskFromCreate(this CreateTaskRequestDTO taskDto){
        return new Task{
            Title = taskDto.Title,
            Description = taskDto.Description,
            DateAdded = taskModel.DateAdded,
            DueDate = taskModel.DueDate,
            ListId = taskDto.ListId
        };
    }


    public static Task ToTaskFromUpdate(this UpdateTaskRequestDto taskDto){
        return new Task{
            Title = taskDto.Title,
            Description = taskDto.Description,
            DateAdded = taskModel.DateAdded,
            DueDate = taskModel.DueDate,
            ListId = taskDto.ListId
        };
    }
}