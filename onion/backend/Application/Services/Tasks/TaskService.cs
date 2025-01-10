using Application.Dtos.TasksDtos;
using Application.Handlers;
using Domain.Interfaces;

namespace Application.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IListRepository _listRepository;
    private readonly ITaskDeleteHandler _deleteHandler;

    public TaskService(ITasksRepository tasksRepository, IListRepository listRepository, ITaskDeleteHandler deleteHandler)
    {
        _tasksRepository = tasksRepository;
        _listRepository = listRepository;
        _deleteHandler = deleteHandler;
    }
    public async Task<List<TaskDto>> GetAllTasks()
    {
        var tasks = await _tasksRepository.GetTasks();
        var tasksDto = new List<TaskDto>();
        foreach (var task in tasks)
        {
            tasksDto.Add(new TaskDto(task));            
        }

        return tasksDto;
    }

    public async Task<TaskDto> GetTaskById(int taskId)
    {
        var tasks = await _tasksRepository.GetTasks(taskId: taskId);
        var task = tasks.FirstOrDefault();
        if (task == null) throw new Exception("Task not found");

        return new TaskDto(task);
    }

    public async Task<List<TaskInfoDto>> GetTasksByWorkspaceId(int workspaceId)
    {
        var tasks = await _tasksRepository.GetTasks(workspaceId: workspaceId);
        var tasksInfoDto = new List<TaskInfoDto>();
        foreach (var task in tasks)
        {
            tasksInfoDto.Add(new TaskInfoDto(task.TaskId,
                                            task.Title,
                                            task.List.Title,
                                            task.List.Board.Title,
                                            task.DueDate));
        }

        return tasksInfoDto;
    }

    public async Task<List<TaskDto>> GetTasksByBoardId(int boardId)
    {
        var tasks = await _tasksRepository.GetTasks(boardId: boardId);
        var tasksDto = new List<TaskDto>();
        foreach (var task in tasks)
        {
            tasksDto.Add(new TaskDto(task));
        }

        return tasksDto;
    }

    public async Task<List<TaskDto>> GetTasksByListId(int listId)
    {
        var tasks = await _tasksRepository.GetTasks(listId: listId);
        var tasksDto = new List<TaskDto>();
        foreach (var task in tasks)
        {
            tasksDto.Add(new TaskDto(task));
        }

        return tasksDto;
    }

    public async Task<TaskDto> UpdateTask(UpdateTaskDto updateTaskDto)
    {
        var tasks = await _tasksRepository.GetTasks(taskId: updateTaskDto.TaskId);
        var task = tasks.FirstOrDefault();
        if (task == null) throw new Exception("Task not found");

        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        task.DueDate = updateTaskDto.DueDate;

        var updatedTask = await _tasksRepository.UpdateTask(task);
        return new TaskDto(updatedTask);
    }

    public async Task<TaskDto> DeleteTask(TaskIdDto taskIdDto)
    {
        var task = (await _tasksRepository.GetTasks(taskId: taskIdDto.TaskId)).FirstOrDefault();
        if (task == null) throw new Exception("Task not found");
        
        await _deleteHandler.HandleDeleteRequest(task.TaskId);
        
        //Create workspace activity

        return new TaskDto(task);
    }

    public async Task<TaskDto> CreateTask(CreateTaskDto createTaskDto)
    {
        var lists = await _listRepository.GetLists(listId: createTaskDto.ListId);
        var list = lists.FirstOrDefault();
        if (list == null) throw new Exception("List not found");

        //Numri i tasks brenda listes - 1 eshte indexi i fundit
        var newIndex = list.Tasks.Count(); 
        
        var newTask = new Domain.Entities.Tasks(newIndex,
                                                createTaskDto.Title,
                                                DateTime.Now,
                                                createTaskDto.ListId);
        
        //CREATE NEW ACTIVITY
        
        return new TaskDto(newTask);
    }

    public Task<TaskDto> DragNDropTask(DragNDropTaskDto dragNDropTaskDto)
    {
        throw new NotImplementedException();
    }
}