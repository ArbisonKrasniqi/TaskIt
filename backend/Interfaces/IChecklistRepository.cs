using backend.DTOs.Checklist.Input;
using backend.Models;

namespace backend.Interfaces;

public interface IChecklistRepository
{
    Task<List<Checklist>> GetAllChecklistsAsync();
    Task<Checklist?> GetChecklistByIdAsync(int checklistId);

    Task<Checklist> CreateChecklistAsync(Checklist checklist);
    Task<Checklist?> UpdateChecklistAsync(UpdateChecklistDTO checklistDto);
    Task<Checklist?> DeleteChecklistAsync(int checklistId);
    Task<List<Checklist>?> GetChecklistByTaskIdAsync(int checklistId);
    Task<List<Checklist>> DeleteChecklistByTaskIdAsync(int taskId);
    Task<bool> ChecklistExists(int checklistId);
    Task<bool> ChecklistInTask(int checklistId, int taskId);
}