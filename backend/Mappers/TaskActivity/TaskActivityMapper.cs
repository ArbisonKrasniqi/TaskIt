using backend.DTOs.TaskActivity.Input;
using backend.DTOs.TaskActivity.Output;

namespace backend.Mappers.TaskActivity;

public static class TaskActivityMapper
{
    public static Models.TaskActivity ToTaskActivityFromCreate(this AddTaskActivityDto taskActivityDto, string userId)
    {
        return new Models.TaskActivity
        {
            TaskId = taskActivityDto.TaskId,
            UserId = userId,
            ActionType = taskActivityDto.ActionType,
            EntityName = taskActivityDto.EntityName
        };
    }

}