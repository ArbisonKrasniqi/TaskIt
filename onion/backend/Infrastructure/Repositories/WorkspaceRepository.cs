using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly AppDbContext _context;

    public WorkspaceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Workspace>> GetWorkspaces(int? workspaceId = null, 
        string ownerId = null, string memberId = null)
    {
        var query = _context.Workspaces
            .Include(w => w.Members) 
            .Include(w => w.Boards) 
            .Include(w => w.Invites) 
            .Include(w => w.Activity) 
            .Include(w => w.StarredBoards) 
            .Include(w => w.User) 
            .AsQueryable();
        if(workspaceId.HasValue)
            query = query.Where(w=>w.WorkspaceId==workspaceId);
        if (!string.IsNullOrEmpty(ownerId))
            query = query.Where(w => w.OwnerId == ownerId);
        if (!string.IsNullOrEmpty(memberId))
            query = query.Where(w => w.Members.Any(m => m.UserId == memberId) || w.OwnerId == ownerId);

        return await query.ToListAsync();
    }

    public async Task<Workspace> CreateWorkspace(Workspace workspace)
    {
        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();
        return workspace;
    }

    public async Task<Workspace> UpdateWorkspace(Workspace workspace)
    {
        var existingWorkspace = await _context.Workspaces.FindAsync(workspace.WorkspaceId);
        if (existingWorkspace == null)
            throw new Exception("Workspace not found");
        
        _context.Entry(existingWorkspace).CurrentValues.SetValues(workspace);
        await _context.SaveChangesAsync();

        return existingWorkspace;
    }

    public async Task<Workspace> DeleteWorkspace(int workspaceId)
    {
        var workspace = await _context.Workspaces.FindAsync(workspaceId);

        if (workspace == null)
            throw new Exception("Workspace not found");

        _context.Workspaces.Remove(workspace);
        await _context.SaveChangesAsync();

        return workspace;
    }
}