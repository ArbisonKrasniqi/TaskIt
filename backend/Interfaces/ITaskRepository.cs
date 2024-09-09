using backend.DTOs.Task;
using backend.Models;

namespace backend.Interfaces;

public interface ITaskRepository{

    Task<List<Tasks>> GetAllTaskAsync();
    Task<Tasks?> GetTaskByIdAsync(int taskId);
    Task<Tasks?> CreateTaskAsync(Tasks taskModel);
    Task<Tasks?> UpdateTaskAsync(UpdateTaskRequestDTO taskDto);
    Task<Tasks?> DeleteTaskAsync(int taskId);

    //Relation with List 

    Task<Tasks> CreateAsync (Tasks taskModel);
    Task<List<Tasks>> DeleteTaskByListIdAsync (int ListId);
    Task<List<Tasks>> GetTaskByListId (int ListId);
    Task<bool> TaskInList(int taskId, int listId);
    Task<List<TaskInfoDto>> GetTasksByWorkspaceIdAsync(int workspaceId);
    Task<List<Tasks>> GetTasksByBoardIdAsync(int boardId);
    Task<bool> TaskExists(int taskId);
    Task<List<Tasks>> FilterClosedBoardTasksAsync(List<Tasks> tasks);
    Task<bool> handleDragNDrop(DragNDropTaskDTO dragNDropTaskDto);
}