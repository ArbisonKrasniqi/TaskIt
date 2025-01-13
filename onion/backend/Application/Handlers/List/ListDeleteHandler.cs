using Domain.Interfaces;

namespace Application.Handlers.List;

public class ListDeleteHandler : IListDeleteHandler
{
    private readonly IListRepository _listRepository;
    private readonly ITaskDeleteHandler _taskDeleteHandler;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepository;
    private readonly UserContext _userContext;
    

    public ListDeleteHandler(IListRepository listRepository, ITaskDeleteHandler taskDeleteHandler,IWorkspaceActivityRepository workspaceActivityRepository, UserContext userContext)
    {
        _listRepository = listRepository;
        _taskDeleteHandler = taskDeleteHandler;
        _workspaceActivityRepository = workspaceActivityRepository;
        _userContext = userContext;
    }

    public async Task HandleDeleteRequest(int listId)
    {
        var list = (await _listRepository.GetLists(listId: listId)).FirstOrDefault();

        var listTasks = list.Tasks.ToList();
        if (listTasks.Any())
        {
            foreach (var task in listTasks)
            {
                await _taskDeleteHandler.HandleDeleteRequest(task.TaskId);
            }
        }
        await _listRepository.DeleteList(listId);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(list.Board.Workspace.WorkspaceId,
            _userContext.Id,
            "Deleted",
            list.Title,
            DateTime.Now);
        await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);
    }

}