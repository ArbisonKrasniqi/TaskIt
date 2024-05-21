using backend.Data;
using backend.DTOs.User.Input;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories;

public class MembersRepository : IMembersRepository
{
    private readonly ApplicationDBContext _context;
    public MembersRepository(ApplicationDBContext context)
    {
        _context = context;
    }
//ADD MEMBERS
    public async Task AddMemberAsync(UserIdDTO userId, WorkspaceIdDto workspaceId)
    {
        var workspace = await _context.Workspace
            .Include(w => w.Members)
            .FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId.WorkspaceId);
        if (workspace == null)
        {
            throw new ArgumentNullException( "Workspace not found!");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId.id);
        if (user == null)
        {
            throw new ArgumentNullException("User not found!");
        }

        if (workspace.Members == null)
        {
            workspace.Members = new List<Models.Members>();
        }

        if (workspace.Members.Any(m => m.UserId == userId.id))
        {
            throw new InvalidOperationException("User is already a member of this workspace!");
        }

        var member = new Models.Members
        {
            UserId = userId.id,
            DateJoined = DateTime.Now,
            WorkspaceId = workspaceId.WorkspaceId,
            User = user
        };

        workspace.Members.Add(member);
        await _context.SaveChangesAsync();
    }
//Get all members of a workspace
    public async Task<List<User>> GetAllMembersAsync(int workspaceId)
    {
        var workspace = await _context.Workspace
            .Include(w => w.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId);
        if (workspace == null)
        {
            throw new ArgumentNullException( "Workspace not found!");
        }

        return workspace.Members.Select(m => m.User).ToList();
    }
    
    //Remove member from workspace
    public async Task<User> RemoveMemberAsync(UserIdDTO userId, WorkspaceIdDto workspaceId)
    {
        var workspace = await _context.Workspace
            .Include(w => w.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId.WorkspaceId);

        if (workspace == null)
        {
            throw new ArgumentNullException("Workspace not found!");
        }

        var member = workspace.Members.FirstOrDefault(m => m.UserId == userId.id);
    
        if (member == null)
        {
            throw new ArgumentNullException("Member not found in this workspace!");
        }

        // Ensure you are not trying to remove the owner
        if (workspace.OwnerId == userId.id)
        {
            throw new Exception("Cannot remove the owner of the workspace");
        }

        // Remove the member
        _context.Members.Remove(member);
        await _context.SaveChangesAsync();

        return member.User;
    }



}

