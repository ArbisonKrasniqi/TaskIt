using backend.DTOs.Task;
using backend.Models;

namespace backend.Interfaces;

public interface ITaskRepository{

    Task<List<Tasks>> GetAllTaskAsync();
    Task<Tasks?> GetTaskByIdAsync(int id);
    Task<Tasks> CreateTaskAsync(Tasks taskModel);
    Task<Tasks?> UpdateTaskAsync(int id, UpdateTaskRequestDTO taskDto);
    Task<Tasks?> DeleteTaskAsync(int id);
    
}