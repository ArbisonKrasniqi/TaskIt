using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DTOs.Label;



namespace backend.Repositories;

public class LabelRepository : ILabelRepository{
    private readonly ApplicationDBContext _context;

    public LabelRepository(ApplicationDBContext context){
        _context = context;
    }

    //gets all the Labels
    public async Task<List<Label>> GetAllLabelAsync(){
        
        return await _context.Label.ToListAsync();
    }

    //it gets the Label by id
    public async Task<Label?> GetLabelByIdAsync(int labelId){

        return await _context.Label.FirstOrDefaultAsync(x => x.LabelId == labelId);
    }

    public async Task<List<Label>> GetLabelsByBoardId(int boardId)
    {
        return await _context.Label.Where(label => label.BoardId == boardId).ToListAsync();
    }

    //creates the Label
    public async Task<Label> CreateLabelAsync(Label labelModel){
         
        await _context.Label.AddAsync(labelModel);
        await _context.SaveChangesAsync();
        return labelModel;
    } 

    //i update-ton Label 
    public async Task<Label?> UpdateLabelAsync(UpdateLabelRequestDTO labelModel){
        
        var existingLabel = await _context.Label.FirstOrDefaultAsync(x => x.LabelId == labelModel.LabelId);
        if(existingLabel == null){
            return null;
        }

        existingLabel.Name = labelModel.Name;

        await _context.SaveChangesAsync();
        return existingLabel;
    }

    //i bon delete Labels
    public async Task<Label?> DeleteLabelAsync(int labelId){
         
        var labelModel = await _context.Label.FirstOrDefaultAsync(x => x.LabelId == labelId);
        if(labelModel == null){
            return null;
        }

        _context.Label.Remove(labelModel);
        await _context.SaveChangesAsync();
        return labelModel;
    }
    
    public async Task<List<Label>> FilterClosedBoardLabelsAsync(List<Label> labels)
    {
        var boardIds = labels.Select(l => l.BoardId)
            .Distinct()
            .ToList();
        
        var closedBoardIds = await _context.Board
            .Where(b => boardIds.Contains(b.BoardId) && b.IsClosed)
            .Select(b => b.BoardId)
            .ToListAsync();

        // Step 3: Filter labels to exclude those associated with closed boards
        var filteredLabels = labels.Where(l => !closedBoardIds.Contains(l.BoardId))
            .ToList();

        return filteredLabels;
    }

    public async Task<List<Label>> GetLabelsByTaskId(int taskId)
    {
        var labels = await _context.TaskLabel
            .Where(tl => tl.TaskId == taskId)
            .Select(tl => tl.Label)
            .ToListAsync();
        return labels;
    }
    //DELETE LABELS BY BOARD ID
    public async Task<List<Label>> DeleteLabelsByBoardId(int boardId)
    {
        var labels = await _context.Label.Where(label => label.BoardId == boardId).ToListAsync();
        _context.Label.RemoveRange(labels);
        await _context.SaveChangesAsync();
        return labels;
    }
}
