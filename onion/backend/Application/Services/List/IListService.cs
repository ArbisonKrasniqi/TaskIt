using Application.Dtos.ListDtos;

namespace Application.Services.List;

public interface IListService
{
    Task<List<ListDto>> GetAllLists();
    Task<ListDto> GetListById(int listId);
    Task<ListDto> GetListByBoardId(int boardId);
    Task<ListDto> UpdateList(UpdateListDto updateListDto);
    Task<ListDto> DragNDropList(DragNDropListDto dragNDropListDto);
    Task<ListDto> DeleteList(ListIdDto listIdDto);
    Task<ListDto> CreateList(CreateListDto listDto);
    // Task<ListDto> DeleteListByBoardId(BoardIdDto boardIdDto);
}