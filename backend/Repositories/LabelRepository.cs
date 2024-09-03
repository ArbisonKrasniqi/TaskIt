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
        existingLabel.Color = labelModel.Color;
        existingLabel.BoardId = labelModel.BoardId;

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
}
