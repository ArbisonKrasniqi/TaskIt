using backend.DTOs.Checklist.Input;
using backend.DTOs.ChecklistItem.Input;
using backend.Models;

namespace backend.Interfaces;

public interface IChecklistItemRepository
{
    Task<List<ChecklistItem>> GetAllChecklistItemsAsync();
    Task<ChecklistItem?> GetChecklistItemByIdAsync(int checklistItemId);
    Task<ChecklistItem> CreateChecklistItemAsync(ChecklistItem checklistItem);
    Task<ChecklistItem?> UpdateChecklistItemAsync(UpdateChecklistItemDTO checklistDto);
    Task<ChecklistItem?> DeleteChecklistItemAsync(int checklistItemId);

    Task<List<ChecklistItem>?> GetChecklistItemByChecklistIdAsync(int checklistId);
    Task<List<ChecklistItem>> DeleteChecklistItemByChecklistIdAsync(int checklistId);

    Task<bool> ChecklistItemExists(int checklistItemId);
    Task<bool> ChecklistItemInChecklist(int checklistItemId, int checklistId);
    Task<ChecklistItem> ChangeChecklistItemChecked(int checklistItemId);
}