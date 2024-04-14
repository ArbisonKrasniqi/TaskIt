using backend.DTOs.List;
using backend.Models;

namespace backend.Interfaces;

public interface IListRepository
{
    Task<List<List>> GetAllListsAsync();

    Task<List?> GetListByIdAsync(int ListId);
    Task<List?> CreateListAsync(List listModel);
    Task<List?> UpdateListAsync(int ListId, UpdateListDTO listDto);
    Task<List?> DeleteListAsync(int ListId);
}