using backend.DTOs.List;
using backend.Models;

namespace backend.Interfaces;

public interface IListRepository
{
    Task<List<List>> GetAllListsAsync();

    Task<List?> GetListByIdAsync(int listId);
    Task<List?> CreateListAsync(List listModel);
    Task<List?> UpdateListAsync(int listId, UpdateListDTO listDto);
    Task<List?> DeleteListAsync(int listId);
}