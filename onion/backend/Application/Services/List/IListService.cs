using Application.Dtos.ListDtos;

namespace Application.Services.List;

public interface IListService
{
    Task<List<ListDto>> GetAllLists();
    Task<ListDto> GetListById(int listId);
    Task<List<ListDto>> GetListByBoardId(int boardId);
    Task<ListDto> UpdateList(UpdateListDto updateListDto);
    Task<ListDto> DragNDroplist(DragNDropListDto dragNDropListDto);
    Task<ListDto> DeleteList(ListIdDto listIdDto);
    Task<ListDto> CreateList(CreateListDto createListDto);
    // Task<ListDto> DeleteListByBoardId(BoardIdDto boardIdDto);
}