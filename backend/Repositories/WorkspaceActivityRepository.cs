using backend.Data;
using backend.DTOs.WorkspaceActivity.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class WorkspaceActivityRepository : IWorkspaceActivityRepository
{
    private readonly ApplicationDBContext _context;

    public WorkspaceActivityRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<WorkspaceActivity> CreateWorkspaceActivityAsync(WorkspaceActivity workspaceActivity)
    {
        workspaceActivity.ActionDate = DateTime.Now;
        await _context.WorkspaceActivity.AddAsync(workspaceActivity);
        await _context.SaveChangesAsync();
        return workspaceActivity;
    }

    public async Task<List<WorkspaceActivityDto>> GetAllWorkspacesActivityAsync()
    {
        var workspacesActivity = await _context.WorkspaceActivity
            .Join(_context.Users,
                wa => wa.UserId,
                u => u.Id,
                (wa, u) => new WorkspaceActivityDto
                {
                    WorkspaceActivityId = wa.WorkspaceActivityId,
                    WorkspaceId = wa.WorkspaceId,
                    UserId = wa.UserId,
                    UserName = u.FirstName,
                    UserLastName = u.LastName,
                    ActionType = wa.ActionType,
                    EntityName = wa.EntityName,
                    ActionDate = wa.ActionDate
                }).ToListAsync();
        
        return workspacesActivity;
    }

    public async Task<WorkspaceActivityDto> GetWorkspaceActivityByIdAsync(int workspaceActivityId)
    {
        var workspaceActivity = await _context.WorkspaceActivity
            .Where(wa => wa.WorkspaceActivityId == workspaceActivityId)
            .Join(_context.Users,
                wa => wa.UserId,
                u => u.Id,
                (wa, u) => new WorkspaceActivityDto
                {
                    WorkspaceActivityId = wa.WorkspaceActivityId,
                    WorkspaceId = wa.WorkspaceId,
                    UserId = wa.UserId,
                    UserName = u.FirstName,
                    UserLastName = u.LastName,
                    ActionType = wa.ActionType,
                    EntityName = wa.EntityName,
                    ActionDate = wa.ActionDate
                }).FirstOrDefaultAsync();
        return workspaceActivity;
    }

    public async Task<List<WorkspaceActivityDto>> GetWorkspaceActivitiesByWorkspace(int workspaceId)
    {
        var workspace = await _context.Workspace.FirstOrDefaultAsync(w=> w.WorkspaceId == workspaceId);
        if (workspace == null)
        {
            return null;
        }

        var workspaceActivity = await _context.WorkspaceActivity
            .Where(wa => wa.WorkspaceId == workspaceId)
            .Join(_context.Users,
                wa => wa.UserId,
                u => u.Id,
                (wa, u) => new WorkspaceActivityDto
                {
                    WorkspaceActivityId = wa.WorkspaceActivityId,
                    WorkspaceId = wa.WorkspaceId,
                    UserId = wa.UserId,
                    UserName = u.FirstName,
                    UserLastName = u.LastName,
                    ActionType = wa.ActionType,
                    EntityName = wa.EntityName,
                    ActionDate = wa.ActionDate
                }).ToListAsync();
        return workspaceActivity;
    }

    public async Task<List<WorkspaceActivity?>> DeleteWorkspaceActivitiesByWorkspace(int workspaceId)
    {
        var workspace = await _context.Workspace.FirstOrDefaultAsync(w=> w.WorkspaceId == workspaceId);
        if (workspace == null)
        {
            return null;
        }
        var workspaceActivities = await _context.WorkspaceActivity
            .Where(wa=>wa.WorkspaceId==workspaceId).ToListAsync();
        
        _context.WorkspaceActivity.RemoveRange(workspaceActivities);
        await _context.SaveChangesAsync();
        return workspaceActivities;
    }

    public async Task<WorkspaceActivity?> DeleteWorkspaceActivityByIdAsync(int workspaceActivityId)
    {
        var workspaceActivity = await _context.WorkspaceActivity.FirstOrDefaultAsync(wa=>wa.WorkspaceActivityId==workspaceActivityId);
        if (workspaceActivity == null)
        {
            return null;
        }

        _context.WorkspaceActivity.Remove(workspaceActivity);
        await _context.SaveChangesAsync();
        return workspaceActivity;
    }

}












