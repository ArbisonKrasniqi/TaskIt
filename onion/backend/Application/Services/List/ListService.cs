using Application.Dtos.ListDtos;
using Application.Handlers.List;
using Application.Services.Token;
using Domain.Interfaces;

namespace Application.Services.List;

public class ListService : IListService
{
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly UserContext _userContext;
    private readonly IListDeleteHandler _deleteHandler;

    public ListService(IListRepository listRepo, IBoardRepository boardRepo,UserContext userContext, IListDeleteHandler deleteHandler)
    {
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _userContext = userContext;
        _deleteHandler = deleteHandler;
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
        var lists = await _listRepo.GetLists(listId: listId);
        var list = lists.FirstOrDefault();
        if (list == null)
        {
            throw new Exception("List not found");
        }

        return new ListDto(list);
    }

    public async Task<List<ListDto>> GetListByBoardId(int boardId)
    {
        var lists = await _listRepo.GetLists(boardId: boardId);

        if (lists == null)
        {
            throw new Exception("List not found");
        }

        var listDtos = new List<ListDto>();
        foreach (var list in lists)
        {
            listDtos.Add(new ListDto(list));
        }

        return listDtos;
    }


    public async Task<ListDto> UpdateList(UpdateListDto updateListDto)
    {
        var lists = await _listRepo.GetLists(listId: updateListDto.ListId);
        var list = lists.FirstOrDefault();
        if (list == null)
        {
            throw new Exception("List not found");
        }

        list.ListId = updateListDto.ListId;
        list.Title = updateListDto.Title;

        var updatedList = await _listRepo.UpdateList(list);
        return new ListDto(updatedList);
    }

    public Task<ListDto> DragNDropList(DragNDropListDto dragNDropListDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ListDto> DeleteList(ListIdDto listIdDto)
    {
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
        var boards = (await _boardRepo.GetBoards(boardId: createListDto.BoardId)).FirstOrDefault();
        if (boards == null)
        {
            throw new Exception("Board not found");
        }

        var newIndex = boards.Lists.Count();
        
        var newList = new Domain.Entities.List(createListDto.Title,newIndex,DateTime.Now,createListDto.BoardId);
        
        return new ListDto(newList);
    }
}