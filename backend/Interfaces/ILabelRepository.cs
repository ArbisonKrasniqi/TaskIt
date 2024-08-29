using backend.DTOs.Label;
using backend.Models;


namespace backend.Interfaces;

public interface ILabelRepository{

    Task<List<Label>> GetAllLabelAsync();
    Task<Label?> GetLabelByIdAsync(int labelId);
    Task<Label?> CreateLabelAsync(Label labelModel);
    Task<Label?> UpdateLabelAsync(UpdateLabelRequestDTO labelDto);
    Task<Label?> DeleteLabelAsync(int labelId);
    
}