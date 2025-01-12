using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WorkspaceActivityRepository : IWorkspaceActivityRepository
{
    private readonly AppDbContext _context;

    public WorkspaceActivityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkspaceActivity>> GetWorkspaceActivity(int? workspaceActivityId = null, int? workspaceId = null, string userId = null)
    {
        var query = _context.WorkspaceActivities.AsQueryable();
        
        if (workspaceActivityId.HasValue)
            query = query.Where(w => w.WorkspaceActivityId == workspaceActivityId);
        if (workspaceId.HasValue)
            query = query.Where(w => w.WorkspaceId == workspaceId);
        if (!string.IsNullOrEmpty(userId))
            query = query.Where(w => w.UserId == userId);

        return await query.ToListAsync();

    }

    public async Task<WorkspaceActivity> CreateWorkspaceActivity(WorkspaceActivity workspaceActivity)
    {
        await _context.WorkspaceActivities.AddAsync(workspaceActivity);
        await _context.SaveChangesAsync();
        return workspaceActivity;
    }

    public async Task<WorkspaceActivity> UpdateWorkspaceActivity(WorkspaceActivity workspaceActivity)
    {
        var existingWorkspaceActivity = await _context.WorkspaceActivities.FindAsync(workspaceActivity.WorkspaceActivityId);
        if (existingWorkspaceActivity == null)
        {
            throw new Exception("Workspace activity not found");
        }
        
        _context.Entry(existingWorkspaceActivity).CurrentValues.SetValues(workspaceActivity);
        await _context.SaveChangesAsync();
        return existingWorkspaceActivity;
    }

    public async Task<WorkspaceActivity> DeleteWorkspaceActivity(int workspaceActivityId)
    {
        var workspaceActivity = await _context.WorkspaceActivities.FindAsync(workspaceActivityId);
        if (workspaceActivity == null)
        {
            throw new Exception("Workspace activity not found");
        }

        _context.WorkspaceActivities.Remove(workspaceActivity);
        await _context.SaveChangesAsync();
        return workspaceActivity;
    }

}