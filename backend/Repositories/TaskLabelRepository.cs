using backend.Data;
using backend.DTOs.TaskLabelDTO.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class TaskLabelRepository : ITaskLabelRepository
{
    private readonly ApplicationDBContext _context;
    private readonly ILabelRepository _labelRepo;

    public TaskLabelRepository(ApplicationDBContext context, ILabelRepository labelRepo )
    {
        _context = context;
        _labelRepo = labelRepo;
    }

    public async Task<List<TaskLabel>> getAllTaskLabelsOpenBoardAsync()
    {
        var uniqueLabels = await _context.TaskLabel
            .Include(tl => tl.Label)
            .Select(tl => tl.Label)
            .GroupBy(l => l.LabelId)
            .Select(g => g.First())
            .ToListAsync();
        
        var labelsInOpenBoards = await _labelRepo.FilterClosedBoardLabelsAsync(uniqueLabels);
        
        var taskLabelsInOpenBoards = await _context.TaskLabel
            .Where(tl => labelsInOpenBoards.Select(l => l.LabelId).Contains(tl.LabelId))
            .ToListAsync();

        return taskLabelsInOpenBoards;
    }

    public async Task<bool> labelIsInOpenBoard(int labelId)
    {
        var label = await _context.Label.FirstOrDefaultAsync(l => l.LabelId == labelId);
        if (label == null) return false;

        var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardId == label.BoardId);
        if (board == null) return false;
        
        return !board.IsClosed;
    }

    public async Task<List<TaskLabel>> getAllTaskLabelsAsync()
    {
        return await _context.TaskLabel.ToListAsync();
    }

    public async Task<TaskLabel> getTaskLabelByIdAsync(int taskLabelId)
    {
        return await _context.TaskLabel.FirstOrDefaultAsync(tl => tl.TaskLabelId == taskLabelId);
    }

    public async Task<TaskLabel> getTaskLabelByLabelAndTask(int labelId, int taskId)
    {
        return await _context.TaskLabel.FirstOrDefaultAsync(tl => tl.LabelId == labelId && tl.TaskId == taskId);
    }

    public async Task<TaskLabel> assignLabelToTask(TaskLabel taskLabelModel)
    {
        await _context.TaskLabel.AddAsync(taskLabelModel);
        await _context.SaveChangesAsync();
        return taskLabelModel;
    }

    public async Task<bool> TaskLabelExists(AssignLabelDTO assignLabelDto)
    {
        var taskLabel = await _context.TaskLabel.FirstOrDefaultAsync(tl =>
            tl.LabelId == assignLabelDto.LabelId && tl.TaskId == assignLabelDto.TaskId);
        if (taskLabel == null)
        {
            return false;
        }

        return true;
    }

    public async Task<TaskLabel> removeTaskLabel(TaskLabel taskLabelModel)
    {
        _context.TaskLabel.Remove(taskLabelModel);
        await _context.SaveChangesAsync();
        return taskLabelModel;
    }
//Delete TaskLabels by task Id
public async Task<List<TaskLabel>> DeleteTaskLabelsByTaskId(int taskId)
{
    var taskLabels = await _context.TaskLabel.Where(tl => tl.TaskId == taskId).ToListAsync();
    _context.TaskLabel.RemoveRange(taskLabels);
    await _context.SaveChangesAsync();
    return taskLabels;
}
}