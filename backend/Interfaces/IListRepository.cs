using backend.DTOs.List;
using backend.Models;

namespace backend.Interfaces;

public interface IListRepository
{
    Task<List<List>> GetAllListsAsync();
    Task<List?> GetListByIdAsync(int listId);
    Task<List?> UpdateListAsync(UpdateListDTO listDto);
    Task<List?> DeleteListAsync(int listId);
    //Relation with the board
    Task<List> CreateAsync(List listModel);
    Task<List<List>> DeleteListsByBoardIdAsync(int BoardId);
    Task<List<List>> GetListByBoardId(int BoardId);
    Task<bool> ListExists(int listId);
    Task<bool> ListInBoard(int listId, int boardId);
    Task<bool> HandleDragNDrop(Models.List list, int newIndex);

}