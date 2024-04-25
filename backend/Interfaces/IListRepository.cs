using backend.DTOs.List;
using backend.Models;

namespace backend.Interfaces;

public interface IListRepository
{
    Task<List<List>> GetAllListsAsync();

    Task<List?> GetListByIdAsync(int listId);
    Task<List?> CreateListAsync(List listModel);
    Task<List?> UpdateListAsync(UpdateListDTO listDto);
    Task<List?> DeleteListAsync(int listId);
    //Relation with the board
    Task<List> CreateAsync(List listModel);
    Task<List<List>> DeleteListsByBoardIdAsync(int BoardId);
    Task<List<List>> GetListByBoardId(int BoardId);

}