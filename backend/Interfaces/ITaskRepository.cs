using backend.DTOs.Task;
using backend.Models;

namespace backend.Interfaces;

public interface ITaskRepository{

    Task<List<Task>> GetAllTaskAsync();
    Task<Task?> GetTaskByIdAsync(int id);
    Task<Task> CreateTaskAsync(Task taskModel);
    Task<Task?> UpdateTaskAsync(int id, UpdateTaskRequestDTO taskDto);
    Task<Task?> DeleteTaskAsync(int id);
    
}