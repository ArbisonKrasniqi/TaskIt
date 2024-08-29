using backend.Data;
using backend.DTOs.Board.Output;
using backend.DTOs.Checklist.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ChecklistRepository : IChecklistRepository
{
    private readonly ApplicationDBContext _context;

    public ChecklistRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<Checklist>> GetAllChecklistsAsync()
    {
        return await _context.Checklist.ToListAsync();
    }

    public async Task<Checklist?> GetChecklistByIdAsync(int checklistId)
    {
        return await _context.Checklist.FirstOrDefaultAsync(c => c.ChecklistId == checklistId);
    }

    public async Task<Checklist> CreateChecklistAsync(Checklist checklistModel)
    {
        await _context.Checklist.AddAsync(checklistModel);
        await _context.SaveChangesAsync();
        return checklistModel;
    }

    public async Task<Checklist?> UpdateChecklistAsync(UpdateChecklistDTO checklistDto)
    {
        var existingChecklist =
            await _context.Checklist.FirstOrDefaultAsync(c => c.ChecklistId == checklistDto.ChecklistId);
        if (existingChecklist == null)
        {
            throw new Exception("Checklist not found");

        }

        existingChecklist.Title = checklistDto.Title;
        existingChecklist.TaskId = checklistDto.TaskId;

        await _context.SaveChangesAsync();
        return existingChecklist;
    }

    public async Task<Checklist?> DeleteChecklistAsync(int checklistId)
    {
        var checklistModel = await _context.Checklist.FirstOrDefaultAsync(c => c.ChecklistId == checklistId);

        if (checklistModel == null)
        {
            return null;
        }
        
        _context.ChecklistItem.RemoveRange(checklistModel.ChecklistItems);
        _context.Checklist.Remove(checklistModel);
        await _context.SaveChangesAsync();
        return checklistModel;
    }
    
    //Relationship with Task 
    
    public async Task<List<Checklist>> GetChecklistByTaskIdAsync(int taskId)
    {
        return await _context.Checklist.Where(c => c.TaskId == taskId).ToListAsync();
    }

    public async Task<List<Checklist>> DeleteChecklistByTaskIdAsync(int taskId)
    {
        var checklists = await _context.Checklist.Where(c => c.TaskId == taskId).ToListAsync();
        
        _context.Checklist.RemoveRange(checklists);
        await _context.SaveChangesAsync();
        return checklists;
    }
    
    //Checklist exists

    public async Task<bool> ChecklistExists(int checklistId)
    {
        return await _context.Checklist.AnyAsync(c => c.ChecklistId == checklistId);
    }
    
    //Checklist in task 
    public async Task<bool> ChecklistInTask(int checklistId, int taskId)
    {
        var checklistModel = await _context.Checklist.FirstOrDefaultAsync(c => c.ChecklistId == checklistId);
        var taskModel = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);

        if (taskModel != null && checklistModel != null)
        {
            if (checklistModel.TaskId == taskId)
            {
                return true;
            }
        }

        return false;
    }
}