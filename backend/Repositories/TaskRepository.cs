using backend.Data;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;


namespace backend.Repositories;

public class TaskRepository : ITaskRepository{
    private readonly ApplicationDBContext _context;

    public WorkspaceRepository(ApplicationDBContext context){
        _context = context;
    }

    public async Task<Task?> GetAllTaskAsync(){
        return await _context.Task.ToListAsync();
    }

    public async Task<Task?> GetTaskByIdAsync(int id){
        return await _context.Task.FindAsync(id);
    }

    public async Task<Task> CreateTaskAsync(Task taskModel){
        await _context.Task.AddAsync(taskModel);
        await _context.SaveChangesAsync();
        return taskModel;
    }

    public async Task<Task?> UpdateTaskAsync (int id, UpdateTaskRequestDTO taskDto){
        var existingTask = await _conext.Task.FirstOrDefaultAsync(x => x.TaskID == id);
        if(existingTask == null){
            return null;
        }

        existingTask.Title = taskDto.Title;
        existingTask.Description = taskDto.Description;
        existingTask.DateAdded = taskDto.DateAdded;
        existingTask.DueDate = taskDto.DueDate;
        existingTask.ListId = taskDto.ListId;
        _context.SaveChanges();
        return Ok(taskModel.ToTaskDto());
    }

    public async Task<Task?> DeleteTaskAsync(int id){
        var taskModel = await _context.Task.FirstOrDefaultAsync(x => x.TaskID = id);
        if(taskModel = null){
            return null;
        }

        _context.Task.Remove(taskModel);
        await _context.SaveChangesAsync();
        return taskModel;
    }

}