using backend.Data;
using backend.DTOs.Task;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace backend.Repositories;

public class TaskRepository : ITaskRepository{
    private readonly ApplicationDBContext _context;

    public TaskRepository(ApplicationDBContext context){
        _context = context;
    }

    public async Task<List<Tasks?>> GetAllTaskAsync(){
        return  await _context.Tasks.ToListAsync();
    }

    public async Task<Tasks?> GetTaskByIdAsync(int id){
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<Tasks> CreateTaskAsync(Tasks taskModel){
        await _context.Tasks.AddAsync(taskModel);
        await _context.SaveChangesAsync();
        return taskModel;
    }

    public async Task<Tasks?> UpdateTaskAsync (int id, UpdateTaskRequestDTO taskDto){
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
        if(existingTask == null){
            return null;
        }

        existingTask.Title = taskDto.Title;
        existingTask.Description = taskDto.Description;
        existingTask.DateAdded = taskDto.DateAdded;
        existingTask.DueDate = taskDto.DueDate;
        existingTask.ListId = taskDto.ListId;
        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<Tasks?> DeleteTaskAsync(int id){
        var taskModel = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
        if(taskModel == null){
            return null;
        }

        _context.Tasks.Remove(taskModel);
        await _context.SaveChangesAsync();
        return taskModel;
    }

  

}