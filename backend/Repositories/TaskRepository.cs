using backend.Data;
using backend.DTOs.Task;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace backend.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDBContext _context;

    public TaskRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<Tasks>> GetAllTaskAsync()
    {

        return await _context.Tasks.ToListAsync();
    }

    public async Task<Tasks?> GetTaskByIdAsync(int taskId)
    {
        return await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == taskId);
    }



public async Task<Tasks> CreateTaskAsync(Tasks taskModel){

        await _context.Tasks.AddAsync(taskModel);
        await _context.SaveChangesAsync();
        return taskModel;
    }


    public async Task<Tasks?> UpdateTaskAsync (UpdateTaskRequestDTO taskModel){
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == taskModel.TaskId);
        if(existingTask == null){
            return null;
        }

        existingTask.Title = taskModel.Title;
        existingTask.Description = taskModel.Description;
        existingTask.DateAdded = existingTask.DateAdded;
        existingTask.DueDate = taskModel.DueDate;
        existingTask.ListId = taskModel.ListId;

        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<Tasks?> DeleteTaskAsync(int taskId){
        var taskModel = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == taskId);
        if(taskModel == null){
            return null;
        }

        _context.Tasks.Remove(taskModel);
        await _context.SaveChangesAsync();
        return taskModel;
    }



    // Relation with List from ITaskRepo

    public async Task<Tasks> CreateAsync (Tasks taskModel) {
        await _context.Tasks.AddAsync(taskModel);
        await _context.SaveChangesAsync();
        return taskModel;
    }

    public async Task<List<Tasks>> DeleteTaskByListIdAsync (int ListId){
        var tasks = await _context.Tasks.Where(x => x.ListId == ListId).ToListAsync();
        if (tasks.Count == 0){
            return null;
        }
        _context.Tasks.RemoveRange(tasks);
        await _context.SaveChangesAsync();
        return tasks;
    }

    public async Task<List<Tasks>> GetTaskByListId (int ListId){
        return await _context.Tasks.Where(x => x.ListId == ListId).ToListAsync();
    }

    public async Task<bool> TaskInList(int taskId, int listId)
    {
        var listModel = await _context.List.FirstOrDefaultAsync(x => x.ListId == listId);
        var taskModel = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == taskId);
        if (listModel != null && taskModel != null)
        {
            if (taskModel.ListId == listId)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<List<TaskInfoDto>> GetTasksByWorkspaceIdAsync(int workspaceId)
    {
        var tasks = await (from workspace in _context.Workspace
            join board in _context.Board on workspace.WorkspaceId equals board.WorkspaceId
            join list in _context.List on board.BoardId equals list.BoardId
            join task in _context.Tasks on list.ListId equals task.ListId
            where workspace.WorkspaceId == workspaceId && board.IsClosed == false
            select new TaskInfoDto
            {
                TaskId = task.TaskId,
                TaskTitle = task.Title,
                ListTitle = list.Title,
                BoardTitle = board.Title,
                DueDate = task.DueDate
            }).ToListAsync();

        return tasks;
    }
    
    public async Task<List<Tasks>> GetTasksByBoardIdAsync(int boardId)
    {
        var tasks = await (from board in _context.Board
            join list in _context.List on board.BoardId equals list.BoardId
            join task in _context.Tasks on list.ListId equals task.ListId
            where board.BoardId == boardId && board.IsClosed == false
            select new Tasks
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                index = task.index,
                DueDate = task.DueDate,
                DateAdded = task.DateAdded,
                ListId = task.ListId
                
            }).ToListAsync();

        return tasks;
    }

    public async Task<bool> TaskExists(int taskId)
    {
        return await _context.Tasks.AnyAsync(t => t.TaskId == taskId);
    }

    public async Task<List<Tasks>> FilterClosedBoardTasksAsync(List<Tasks> tasks)
    {
        var boardIds = tasks.Select(t => 
             _context.List
                    .Where(l => l.ListId == t.ListId)
                    .Select(l => l.BoardId)
                    .FirstOrDefault())
            .Distinct()
            .ToList();
        
        var closedBoardIds = await _context.Board
            .Where(b => boardIds.Contains(b.BoardId) && b.IsClosed)
            .Select(b => b.BoardId)
            .ToListAsync();
        
        var filteredTasks = tasks.Where(t => 
            !closedBoardIds.Contains(
                _context.List
                    .Where(l => l.ListId == t.ListId)
                    .Select(l => l.BoardId)
                    .FirstOrDefault())
        ).ToList();

        return filteredTasks;
    }
}