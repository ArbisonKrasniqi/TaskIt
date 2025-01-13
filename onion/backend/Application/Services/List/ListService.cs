using Application.Dtos.ListDtos;
using Application.Handlers.List;
using Application.Services.Authorization;
using Application.Services.Token;
using Domain.Interfaces;

namespace Application.Services.List;

public class ListService : IListService
{
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly UserContext _userContext;
    private readonly IListDeleteHandler _deleteHandler;
    private readonly IAuthorizationService _authorizationService;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepo;

    public ListService(IListRepository listRepo, IBoardRepository boardRepo,UserContext userContext, IListDeleteHandler deleteHandler,IAuthorizationService authorizationService,IWorkspaceActivityRepository workspaceActivityRepo)
    {
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _userContext = userContext;
        _deleteHandler = deleteHandler;
        _authorizationService = authorizationService;
        _workspaceActivityRepo = workspaceActivityRepo;
    }

    public async Task<List<ListDto>> GetAllLists()
    {
        var lists = await _listRepo.GetLists();
        var listsDto = new List<ListDto>();
        foreach (var list in lists)
        {
            listsDto.Add(new ListDto(list));
        }

        return listsDto;
    }

    public async Task<ListDto> GetListById(int listId)
    {
        if (!await _authorizationService.CanAccessList(_userContext.Id, listId))
            throw new Exception("You are not authorized");
        
        var list = (await _listRepo.GetLists(listId: listId)).FirstOrDefault();
        if (list == null)
        {
            throw new Exception("List not found");
        }

        return new ListDto(list);
    }

    public async Task<List<ListDto>> GetListByBoardId(int boardId)
    {
        var accessBoard = await _authorizationService.CanAccessBoard(_userContext.Id, boardId);
        if(!accessBoard && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var lists = await _listRepo.GetLists(boardId: boardId);
        var listDtos = new List<ListDto>();
        foreach (var list in lists)
        {
            listDtos.Add(new ListDto(list));
        }

        return listDtos;
    }
    
    public async Task<ListDto> UpdateList(UpdateListDto updateListDto)
    {
        var accessList = await _authorizationService.CanAccessList(_userContext.Id, updateListDto.ListId);
        if (!accessList && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var list = (await _listRepo.GetLists(listId: updateListDto.ListId)).FirstOrDefault();
        if (list == null)
        {
            throw new Exception("List not found");
        }

        list.ListId = updateListDto.ListId;
        list.Title = updateListDto.Title;

        var updatedList = await _listRepo.UpdateList(list);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(updatedList.Board.Workspace.WorkspaceId,
            _userContext.Id,
            "Created",
            updatedList.Title,
            DateTime.Now);
        await _workspaceActivityRepo.CreateWorkspaceActivity(newActivity);
        
        return new ListDto(updatedList);
    }

    public Task<ListDto> DragNDropList(DragNDropListDto dragNDropListDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ListDto> DeleteList(ListIdDto listIdDto)
    {
        var accessList = await _authorizationService.CanAccessList(_userContext.Id, listIdDto.ListId);
        if (!accessList && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var list = (await _listRepo.GetLists(listId: listIdDto.ListId)).FirstOrDefault();
        if (list == null)
        {
            throw new Exception("List not found");
        }

        await _deleteHandler.HandleDeleteRequest(list.ListId);
        
        return new ListDto(list);
    }

    public async Task<ListDto> CreateList(CreateListDto createListDto)
    {
        var accessBoard = await _authorizationService.CanAccessBoard(_userContext.Id, createListDto.BoardId);
        if(!accessBoard && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var boards = (await _boardRepo.GetBoards(boardId: createListDto.BoardId)).FirstOrDefault();
        if (boards == null)
        {
            throw new Exception("Board not found");
        }

        var newIndex = boards.Lists.Count() ;
        var list = new Domain.Entities.List(createListDto.Title,newIndex,DateTime.Now,createListDto.BoardId);
        var newList = await _listRepo.CreateList(list);
        var newActivity = new Domain.Entities.WorkspaceActivity(newList.Board.Workspace.WorkspaceId,
            _userContext.Id,
            "Created",
            newList.Title,
            DateTime.Now);
        await _workspaceActivityRepo.CreateWorkspaceActivity(newActivity);
        
        
        return new ListDto(newList);
    }
}