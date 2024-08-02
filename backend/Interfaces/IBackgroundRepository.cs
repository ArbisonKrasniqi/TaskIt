using backend.DTOs.Background.Input;
using backend.Models;

namespace backend.Interfaces
{
    public interface IBackgroundRepository
    {
        Task<List<Background>> GetAllBackgroundsAsync();
        Task<Background?> GetBackgroundByIdAsync(int id);
        Task<List<Background>> GetBackgroundsByCreatorIdAsync(string creatorId);
        Task<List<Background>> GetActiveBackgroundsAsync();
        Task<Background> CreateBackgroundAsync(Background background);
        Task<Background?> UpdateBackgroundByIdAsync(UpdateBackgroundDto backgroundDto);
        Task<Background?> DeleteBackgroundByIdAsync(int id);
        Task<List<Background>> DeleteBackgroundsByCreatorId(string creatorId);
        Task<bool> BackgroundExists(int backgroundId);
    }
}