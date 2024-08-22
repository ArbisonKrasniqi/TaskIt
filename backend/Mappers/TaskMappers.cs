using backend.DTOs.Task;
using backend.Models;

namespace backend.Mappers;

public static class TaskMappers{

    public static TaskDto ToTaskDto (this Tasks taskModel){
        return new TaskDto{
            TaskID = taskModel.TaskId,
            Title = taskModel.Title,
            Description = taskModel.Description,
            ListId = taskModel.ListId,
            DueDate = taskModel.DueDate,
            DateAdded = taskModel.DateAdded
        };
    }


    public static Tasks ToTaskFromCreate(this CreateTaskRequestDTO taskDto){
        return new Tasks{
            Title = taskDto.Title,
            Description = taskDto.Description,
            DateAdded = taskDto.DateAdded,
            DueDate = taskDto.DueDate,
            ListId = taskDto.ListId
        };
    }


    public static Tasks ToTaskFromUpdate(this UpdateTaskRequestDTO taskDto){
        return new Tasks{
            Title = taskDto.Title,
            Description = taskDto.Description,
            DateAdded = taskDto.DateAdded,
            DueDate = taskDto.DueDate,
            ListId = taskDto.ListId
        };
    }
}