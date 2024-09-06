using backend.DTOs.Task;
using backend.DTOs.TaskMember.Output;
using backend.Models;

namespace backend.Mappers;

public static class TaskMappers{

    public static TaskDto ToTaskDto (this Tasks taskModel, List<Models.Label> labels, List<TaskMemberDto> taskMembers){
        return new TaskDto{
            TaskId = taskModel.TaskId,
            Title = taskModel.Title,
            Description = taskModel.Description,
            ListId = taskModel.ListId,
            DueDate = taskModel.DueDate,
            DateAdded = taskModel.DateAdded,
            Labels = labels,
            TaskMembers = taskMembers
        };
    }

    public static TaskInfoDto2 toTaskInfoDto2(this TaskInfoDto taskModel, List<Models.Label> labels, List<TaskMemberDto> taskMembers)
    {
        return new TaskInfoDto2
        {
            TaskId = taskModel.TaskId,
            TaskTitle = taskModel.TaskTitle,
            ListTitle = taskModel.ListTitle,
            BoardTitle = taskModel.BoardTitle,
            DueDate = taskModel.DueDate,
            Labels = labels,
            TaskMembers = taskMembers
        };
    } 

    public static TaskDtoLabels ToTaskDtoLabels(this Tasks taskModel, List<Models.Label> labels)
    {
        return new TaskDtoLabels
        {
            TaskId = taskModel.TaskId,
            Title = taskModel.Title,
            Description = taskModel.Description,
            ListId = taskModel.ListId,
            DueDate = taskModel.DueDate,
            DateAdded = taskModel.DateAdded,
            Labels = labels
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