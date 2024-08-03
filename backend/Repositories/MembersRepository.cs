using backend.Data;
using backend.DTOs.Members;
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
    public async Task AddMemberAsync(AddMemberDto addMemberDto)
    {
        var workspace = await _context.Workspace
            .Include(w => w.Members)
            .FirstOrDefaultAsync(x => x.WorkspaceId == addMemberDto.WorkspaceId);
        if (workspace == null)
        {
            throw new ArgumentNullException( "Workspace not found!");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == addMemberDto.UserId);
        if (user == null)
        {
            throw new ArgumentNullException("User not found!");
        }

        if (workspace.Members == null)
        {
            workspace.Members = new List<Models.Members>();
        }

        if (workspace.Members.Any(m => m.UserId == addMemberDto.UserId))
        {
            throw new InvalidOperationException("User is already a member of this workspace!");
        }

        var member = new Models.Members
        {
            UserId = addMemberDto.UserId,
            DateJoined = DateTime.Now,
            WorkspaceId = addMemberDto.WorkspaceId,
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
    public async Task<User> RemoveMemberAsync(int workspaceId, string memberId)
    {
        var workspace = await _context.Workspace
            .Include(w => w.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId);

        if (workspace == null)
        {
            throw new ArgumentNullException("Workspace not found!");
        }

        var member = workspace.Members.FirstOrDefault(m => m.UserId == memberId);
    
        if (member == null)
        {
            throw new ArgumentNullException("Member not found in this workspace!");
        }

        // Ensure you are not trying to remove the owner
        if (workspace.OwnerId == memberId)
        {
            throw new Exception("Cannot remove the owner of the workspace");
        }

        // Remove the member
        _context.Members.Remove(member);
        await _context.SaveChangesAsync();

        return member.User;
    }



}

