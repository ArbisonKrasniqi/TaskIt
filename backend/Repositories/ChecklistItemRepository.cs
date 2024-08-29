using backend.Data;
using backend.DTOs.ChecklistItem.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ChecklistItemRepository:IChecklistItemRepository
{
    private readonly ApplicationDBContext _context;

    public ChecklistItemRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<ChecklistItem>> GetAllChecklistItemsAsync()
    {
        return await _context.ChecklistItem.ToListAsync();
    }

    public async Task<ChecklistItem?> GetChecklistItemByIdAsync(int checklistItemId)
    {
        return await _context.ChecklistItem.FirstOrDefaultAsync(c => c.ChecklistItemId == checklistItemId);
    }

    public async Task<ChecklistItem> CreateChecklistItemAsync(ChecklistItem checklistItemModel)
    {
        await _context.ChecklistItem.AddAsync(checklistItemModel);
        await _context.SaveChangesAsync();
        return checklistItemModel;
    }

    public async Task<ChecklistItem?> UpdateChecklistItemAsync(UpdateChecklistItemDTO checklistItemDto)
    {
        var existingChecklistItem =
            await _context.ChecklistItem.FirstOrDefaultAsync(c =>
                c.ChecklistItemId == checklistItemDto.ChecklistItemId);
        if (existingChecklistItem == null)
        {
            throw new Exception("ChecklistItem not found");
        }

        existingChecklistItem.Content = checklistItemDto.Content;
        existingChecklistItem.ChecklistId = checklistItemDto.ChecklistId;

        await _context.SaveChangesAsync();
        return existingChecklistItem;
    }

    public async Task<ChecklistItem?> DeleteChecklistItemAsync(int checklistItemId)
    {
        var checklistItemModel =
            await _context.ChecklistItem.FirstOrDefaultAsync(c => c.ChecklistItemId == checklistItemId);
        if (checklistItemModel == null)
        {
            return null;
        }

        _context.ChecklistItem.Remove(checklistItemModel);
        await _context.SaveChangesAsync();
        return checklistItemModel;
    }
    //Relation with Checklist
    public async Task<List<ChecklistItem>?> GetChecklistItemByChecklistIdAsync(int checklistId)
    {
        return await _context.ChecklistItem.Where(c => c.ChecklistId == checklistId).ToListAsync();
    }

    public async Task<List<ChecklistItem>> DeleteChecklistItemByChecklistIdAsync(int checklistId)
    {
        var checklistItems = await _context.ChecklistItem.Where(c => c.ChecklistId == checklistId).ToListAsync();
        
        _context.ChecklistItem.RemoveRange(checklistItems);
        await _context.SaveChangesAsync();
        return checklistItems;
    }
    
    //ChecklistItem exists

    public async Task<bool> ChecklistItemExists(int checklistItemId)
    {
        return await _context.ChecklistItem.AnyAsync(c => c.ChecklistItemId == checklistItemId);
    }
    
    //ChecklistItem in checklist
    public async Task<bool> ChecklistItemInChecklist(int checklistItemId, int checklistId)
    {
        var checklistItemModel =
            await _context.ChecklistItem.FirstOrDefaultAsync(c => c.ChecklistItemId == checklistItemId);
        var checklistModel = await _context.Checklist.FirstOrDefaultAsync(c => c.ChecklistId == checklistId);

        if (checklistModel != null && checklistItemModel != null)
        {
            if (checklistItemModel.ChecklistId == checklistId)
            {
                return true;
            }
        }

        return false;
    }

}