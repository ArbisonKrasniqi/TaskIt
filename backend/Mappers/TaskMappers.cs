using backend.DTOs.Task;
using backend.Models;

namespace backend.Mappers;

public static class TaskMappers{

    public static TaskDto ToTaskDto (this Tasks taskModel){
        return new TaskDto{
            TaskID = taskModel.TaskId,
            Title = taskModel.Title,
            Description = taskModel.Description,
            DateAdded = taskModel.DateAdded,
            DueDate = taskModel.DueDate,
            ListId = taskModel.ListId
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