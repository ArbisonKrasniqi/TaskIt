using backend.Data;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDBContext _context;
    public UserRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<bool> UserExists(string id)
    {
        return await _context.Users.AnyAsync(s => s.Id.Equals(id));
    }

    public async Task<bool> UserOwnsWorkspace(string userId, int? workspaceId)
    {
        //Check if user owns workspace
        var workspaceModel =
            await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.OwnerId == userId);
        return workspaceModel != null;
    }

    public async Task<bool> UserIsMember(string userId, int workspaceId)
    {
        var workspaceModel =
            await _context.Members.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.UserId == userId);
        return workspaceModel != null;
    }
}