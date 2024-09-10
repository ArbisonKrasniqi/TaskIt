using backend.Data;
using backend.DTOs.TaskActivity.Input;
using backend.DTOs.TaskActivity.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class TaskActivityRepository : ITaskActivityRepository
{
    private readonly ApplicationDBContext _context;

    public TaskActivityRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<TaskActivity> CreateTaskActivityAsync(TaskActivity taskActivity)
    {
        taskActivity.ActionDate = DateTime.Now;
        await _context.TaskActivity.AddAsync(taskActivity);
        await _context.SaveChangesAsync();
        return taskActivity;
    }

    public async Task<List<TaskActivityDto>> GetAllTasksActivityAsync()
    {
        var taskActivity = await _context.TaskActivity
            .Join(_context.Users,
                ta => ta.UserId,
                u => u.Id,
                (ta, u) => new TaskActivityDto
                {
                    TaskActivityId = ta.TaskActivityId,
                    TaskId = ta.TaskId,
                    UserId = ta.UserId,
                    UserFirstName = u.FirstName,
                    UserLastName = u.LastName,
                    ActionType = ta.ActionType,
                    EntityName = ta.EntityName,
                    ActionDate = ta.ActionDate
                }).ToListAsync();

        return taskActivity;
    }

    public async Task<TaskActivityDto> GetTaskActivityByIdAsync(int taskActivityId)
    {
        var taskActivity = await _context.TaskActivity
            .Join(_context.Users,
                ta => ta.UserId,
                u => u.Id,
                (ta, u) => new TaskActivityDto
                {
                    TaskActivityId = ta.TaskActivityId,
                    TaskId = ta.TaskId,
                    UserId = ta.UserId,
                    UserFirstName = u.FirstName,
                    UserLastName = u.LastName,
                    ActionType = ta.ActionType,
                    EntityName = ta.EntityName,
                    ActionDate = ta.ActionDate
                }).FirstOrDefaultAsync();

        return taskActivity;
    }
    
    public async Task<List<TaskActivityDto>> GetTaskActivitiesByTaskId(int taskId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t=> t.TaskId == taskId);
        if (task == null)
        {
            return null;
        }

        var taskActivity = await _context.TaskActivity
            .Where(ta => ta.TaskId == taskId)
            .Join(_context.Users,
                ta => ta.UserId,
                u => u.Id,
                (ta, u) => new TaskActivityDto
                {
                    TaskActivityId = ta.TaskActivityId,
                    TaskId = ta.TaskId,
                    UserId = ta.UserId,
                    UserFirstName = u.FirstName,
                    UserLastName = u.LastName,
                    ActionType = ta.ActionType,
                    EntityName = ta.EntityName,
                    ActionDate = ta.ActionDate
                }).ToListAsync();
        return taskActivity;
    }
    
    public async Task<List<TaskActivity?>> DeleteTaskActivitiesByTaskId(int taskId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t=> t.TaskId == taskId);
        if (task == null)
        {
            return null;
        }
        var taskActivities = await _context.TaskActivity
            .Where(ta=>ta.TaskId==taskId).ToListAsync();
        
        _context.TaskActivity.RemoveRange(taskActivities);
        await _context.SaveChangesAsync();
        return taskActivities;
    }
    
    public async Task<TaskActivity?> DeleteTaskActivityByIdAsync(int taskActivityId)
    {
        var taskActivity = await _context.TaskActivity.FirstOrDefaultAsync(ta=>ta.TaskActivityId==taskActivityId);
        if (taskActivity == null)
        {
            return null;
        }

        _context.TaskActivity.Remove(taskActivity);
        await _context.SaveChangesAsync();
        return taskActivity;
    }
}