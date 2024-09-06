using backend.DTOs.Label;
using backend.Models;


namespace backend.Interfaces;

public interface ILabelRepository{

    Task<List<Label>> GetAllLabelAsync();
    Task<Label?> GetLabelByIdAsync(int labelId);
    Task<List<Label>> GetLabelsByBoardId(int boardId);
    Task<Label?> CreateLabelAsync(Label labelModel);
    Task<Label?> UpdateLabelAsync(UpdateLabelRequestDTO labelDto);
    Task<Label?> DeleteLabelAsync(int labelId);
    Task<List<Label>> FilterClosedBoardLabelsAsync(List<Label> labels);
    Task<List<Label>> GetLabelsByTaskId(int taskId);

}