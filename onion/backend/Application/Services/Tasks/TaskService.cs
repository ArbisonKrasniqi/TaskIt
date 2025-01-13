using Application.Dtos.TasksDtos;
using Application.Handlers;
using Application.Services.Authorization;
using Domain.Interfaces;

namespace Application.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IListRepository _listRepository;
    private readonly ITaskDeleteHandler _deleteHandler;
    private readonly UserContext _userContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepository;

    public TaskService(ITasksRepository tasksRepository, IListRepository listRepository, ITaskDeleteHandler deleteHandler, UserContext userContext, IAuthorizationService authorizationService, IWorkspaceActivityRepository workspaceActivityRepository)
    {
        _tasksRepository = tasksRepository;
        _listRepository = listRepository;
        _deleteHandler = deleteHandler;
        _userContext = userContext;
        _authorizationService = authorizationService;
        _workspaceActivityRepository = workspaceActivityRepository;
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
        if (!await _authorizationService.CanAccessTask(_userContext.Id, taskId))
            throw new Exception("You are not authorized");
        
        var task = (await _tasksRepository.GetTasks(taskId: taskId)).FirstOrDefault();
        if (task == null) throw new Exception("Task not found");

        return new TaskDto(task);
    }

    public async Task<List<TaskInfoDto>> GetTasksByWorkspaceId(int workspaceId)
    {
        var isMember = await _authorizationService.IsMember(_userContext.Id, workspaceId);
        var isOwner = await _authorizationService.OwnsWorkspace(_userContext.Id, workspaceId);
        if (!isMember && !isOwner && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
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
        var accessesBoard = await _authorizationService.CanAccessBoard(_userContext.Id, boardId);
        if (!accessesBoard  && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
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
        var accessesList = await _authorizationService.CanAccessList(_userContext.Id, listId);
        if (!accessesList  && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
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
        var accessesTask = await _authorizationService.CanAccessTask(_userContext.Id, updateTaskDto.TaskId);
        if (!accessesTask && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var task = (await _tasksRepository.GetTasks(taskId: updateTaskDto.TaskId)).FirstOrDefault();
        if (task == null) throw new Exception("Task not found");

        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        task.DueDate = updateTaskDto.DueDate;

        var updatedTask = await _tasksRepository.UpdateTask(task);
        
        //Create activity
        var newActivity = new Domain.Entities.WorkspaceActivity(updatedTask.List.Board.Workspace.WorkspaceId,
                                                                 _userContext.Id,
                                                                 "Updated",
                                                                 updatedTask.Title,
                                                                 DateTime.Now);
        await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);
        
        return new TaskDto(updatedTask);
    }

    public async Task<TaskDto> DeleteTask(TaskIdDto taskIdDto)
    {
        var accessesTask = await _authorizationService.CanAccessTask(_userContext.Id, taskIdDto.TaskId);
        if (!accessesTask && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var task = (await _tasksRepository.GetTasks(taskId: taskIdDto.TaskId)).FirstOrDefault();
        if (task == null) throw new Exception("Task not found");
        
        //Handler krijon activity
        await _deleteHandler.HandleDeleteRequest(task.TaskId);
        

        return new TaskDto(task);
    }

    public async Task<TaskDto> CreateTask(CreateTaskDto createTaskDto)
    {
        var accessesList = await _authorizationService.CanAccessList(_userContext.Id, createTaskDto.ListId);
        if (!accessesList && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var list = (await _listRepository.GetLists(listId: createTaskDto.ListId)).FirstOrDefault();
        if (list == null) throw new Exception("List not found");

        //Numri i tasks brenda listes - 1 eshte indexi i fundit
        var newIndex = list.Tasks.Count(); 
        
        var newTask = new Domain.Entities.Tasks(newIndex,
                                                createTaskDto.Title,
                                                DateTime.Now,
                                                createTaskDto.ListId);
        
        //Create activity
         var newActivity = new Domain.Entities.WorkspaceActivity(newTask.List.Board.Workspace.WorkspaceId,
             _userContext.Id,
             "Created",
             newTask.Title,
             DateTime.Now);
         await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);
        
        return new TaskDto(newTask);
    }

    public async Task<TaskDto> DragNDropTask(DragNDropTaskDto dragNDropTaskDto)
    {
        var accessesTask = await _authorizationService.CanAccessTask(_userContext.Id, dragNDropTaskDto.TaskId);
        var accessesList = await _authorizationService.CanAccessList(_userContext.Id, dragNDropTaskDto.ListId);
        if (!accessesTask && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");

        if (!accessesList && _userContext.Role != "Admin")
            throw new Exception("You are not authorized");
        
        var task = (await _tasksRepository.GetTasks(taskId: dragNDropTaskDto.TaskId)).FirstOrDefault();
        if (task == null) throw new Exception("Task not found");
        var list = task.List;

        var newList = (await _listRepository.GetLists(listId: dragNDropTaskDto.ListId)).FirstOrDefault();
        if (newList == null) throw new Exception("New list not found");

        if (newList.BoardId != list.BoardId) throw new Exception("Cannot move task between boards");
        var newIndex = dragNDropTaskDto.newIndex;
        //SameList dragNdrop
        if (list.ListId == newList.ListId)
        {
            var tasks = list.Tasks;
            var currentIndex = task.Index;
            if (tasks.Count < newIndex) throw new Exception("New index out of bounds");
            
            //Nese ska livrit hiq
            if (currentIndex == newIndex) return new TaskDto(task);
            
            for (int i = Math.Min(currentIndex, newIndex); i <= Math.Max(currentIndex, newIndex); i++)
            {
                if (tasks[i] == task) continue; //Vazhdo me task tjeter
                if (currentIndex < newIndex)
                {
                    tasks[i].Index -= 1;
                    await _tasksRepository.UpdateTask(tasks[i]);
                }
                if (currentIndex > newIndex)
                {
                    tasks[i].Index += 1;
                    await _tasksRepository.UpdateTask(tasks[i]);
                }
            }

            task.Index = newIndex;
            var updatedTask = await _tasksRepository.UpdateTask(task);
            
            return new TaskDto(updatedTask);
        }
        else
        //DifferentList drag n drop
        {
            var tasks = newList.Tasks;
            if (tasks.Count < newIndex) throw new Exception("New index out of bounds");
            
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Index >= newIndex)
                {
                    tasks[i].Index += 1;
                    await _tasksRepository.UpdateTask(tasks[i]);
                }
            }

            var oldTasks = task.List.Tasks.Where(t => t.Index > task.Index).OrderBy(t => task.Index);
            foreach (var t in oldTasks)
            {
                t.Index -= 1;
                await _tasksRepository.UpdateTask(t);
            }

            task.ListId = newList.ListId;
            task.Index = newIndex;
            var updatedTask = await _tasksRepository.UpdateTask(task);
            
            //Create activity
            var newActivity = new Domain.Entities.WorkspaceActivity(
                     task.List.Board.Workspace.WorkspaceId,
                     _userContext.Id,
                     "Moved",
                     "Task "+task.Title+" to list "+newList.Title,
                     DateTime.Now
                 );
            await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);
            
            return new TaskDto(updatedTask);
        }
    }
}