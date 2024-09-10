using backend.DTOs.TaskActivity.Output;
using backend.DTOs.WorkspaceActivity.Output;
using backend.Models;

namespace backend.Interfaces;

public interface ITaskActivityRepository
{
    Task<TaskActivity> CreateTaskActivityAsync(TaskActivity taskActivity);
    Task<List<TaskActivityDto>> GetAllTasksActivityAsync();
    Task<TaskActivityDto> GetTaskActivityByIdAsync(int taskActivityId);
    Task<List<TaskActivityDto>> GetTaskActivitiesByTaskId(int taskId);
    Task<List<TaskActivity?>> DeleteTaskActivitiesByTaskId(int taskId);
    Task<TaskActivity?> DeleteTaskActivityByIdAsync(int taskActivityId);
}