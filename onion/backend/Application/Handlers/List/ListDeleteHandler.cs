using Domain.Interfaces;

namespace Application.Handlers.List;

public class ListDeleteHandler : IListDeleteHandler
{
    private readonly IListRepository _listRepository;
    private readonly ITaskDeleteHandler _taskDeleteHandler;

    public ListDeleteHandler(IListRepository listRepository, ITaskDeleteHandler taskDeleteHandler)
    {
        _listRepository = listRepository;
        _taskDeleteHandler = taskDeleteHandler;
    }

    public async Task HandleDeleteRequest(int listId)
    {
        var listTasks = (await _listRepository.GetLists(listId: listId)).FirstOrDefault().Tasks;
        if (listTasks.Any())
        {
            foreach (var task in listTasks)
            {
                await _taskDeleteHandler.HandleDeleteRequest(task.TaskId);
            }
        }

        await _listRepository.DeleteList(listId);
    }

}