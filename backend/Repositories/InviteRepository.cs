using backend.Data;
using backend.DTOs.Invite.Input;
using backend.DTOs.Members;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;


namespace backend.Repositories;

public class InviteRepository : IInviteRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IMembersRepository _memberRepo;

    public InviteRepository(ApplicationDBContext context, IMembersRepository memberRepo)
    {
        _context = context;
        _memberRepo = memberRepo;
    }
    
    //GetAllAsync 
    public async Task<List<Invite>> GetAllInvitesAsync()
    {
        return await _context.Invite.ToListAsync();
    }
    
    //GetByIdAsync
    public async Task<Invite?> GetInviteByIdAsync(int id)
    {
        return await _context.Invite.FirstOrDefaultAsync(i => i.InviteId == id);
    }
    
    //GetByInviter
    public async Task<List<Invite?>> GetInvitesByInviterAsync(string inviterId)
    {
        return await _context.Invite.Where(i=>i.InviterId ==inviterId).ToListAsync();
    }
    
    //GetByInvitee
    public async Task<List<Invite?>> GetInvitesByInviteeAsync(string inviteeId)
    {
        return await _context.Invite.Where(i=>i.InviteeId==inviteeId).ToListAsync();
    }   
    
    //GetInvitesByWorkspace
    public async Task<List<Invite>> GetInvitesByWorkspaceAsync(int id)
    {
        return await _context.Invite.Where(i => i.WorkspaceId == id).ToListAsync();
    }
    
    //GetPending invites by inviter ////merri invites qe ika dergu po nuk ia kan pranu ende
    public async Task<List<Invite>> GetPendingInvitesByInviter(string userId)
    {
        return await _context.Invite.Where(i => i.InviterId == userId && i.InviteStatus == "Pending").ToListAsync();
    }
    //Get Pending invites by invitee ////merri invites qe  nuk i ka pranu ende 
    public async Task<List<Invite>> GetPendingInvitesByInvitee(string userId)
    {
        return await _context.Invite.Where(i => i.InviteeId == userId && i.InviteStatus == "Pending").ToListAsync();
    }

    //InviteExists
    public async Task<bool> InviteExistsAsync(Invite invite)
    {
        return await _context.Invite.AnyAsync(i =>
            i.InviterId == invite.InviterId && i.InviteeId == invite.InviteeId 
                                            && i.WorkspaceId == invite.WorkspaceId
                                            && i.InviteStatus== invite.InviteStatus);
    }
    
    //AddInviteAsync
    public async Task<Invite> AddInviteAsync(Invite invite)
    {    
        var inviteExists = await InviteExistsAsync(invite);
    
        if (inviteExists)
        {
            // Handle the case where the invite already exists
            throw new InvalidOperationException("Invite already exists.");
        }

        var workspace = await _context.Workspace.Include(w => w.Members)
            .FirstOrDefaultAsync(x => x.WorkspaceId == invite.WorkspaceId);
        
        if (workspace == null)
        {
            throw new ArgumentNullException("Workspace not found!");
        }
        if (workspace.Members.Count >= 10)
        {
            throw new InvalidOperationException("Cannot invite more members. Workspace has reached the member limit.");
        }
        _context.Invite.Add(invite);
        await _context.SaveChangesAsync();

        return invite;
    }
    
    //UpdateInviteStatus
    public async Task<Invite?> UpdateInviteStatusAsync(UpdateInviteDto updateInviteDto)
    {
        var invite = await _context.Invite.FindAsync(updateInviteDto.InviteId);
        if (invite == null)
        {
            return null;
        }

        invite.InviteStatus = updateInviteDto.InviteStatus; //accept ose decline psh

        if (invite.InviteStatus == "Accepted")
        {
            var addMemberDto = new AddMemberDto
            {
                UserId = invite.InviteeId,
                WorkspaceId = invite.WorkspaceId
            };
            try
            {
                await _memberRepo.AddMemberAsync(addMemberDto);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed to add member to the workspace");
            }

        }
        await _context.SaveChangesAsync();
        return invite;
    }
    
    //DeleteInviteAsync
    public async Task<bool> DeleteInviteAsync(int inviteId)
    {
        var invite = await _context.Invite.FindAsync(inviteId);
        if (invite == null) return false;
        
        _context.Invite.Remove(invite);
        await _context.SaveChangesAsync();
        return true;
    }

    //UPDATE INVITE
    public async Task<Invite?> UpdateInviteAsync(UpdateInviteAdminDto updateDto)
    {
        var existingInvite = await _context.Invite.FirstOrDefaultAsync(i => i.InviteId == updateDto.InviteId);
        if (existingInvite == null) return null;

        existingInvite.InviterId = updateDto.InviterId;
        existingInvite.InviteeId = updateDto.InviteeId;
        existingInvite.WorkspaceId = updateDto.WorkspaceId;
        existingInvite.InviteStatus = updateDto.InviteStatus;
        await _context.SaveChangesAsync();
        return existingInvite;
    }
    
    //DELETE INVITES BY WORKSPACEID
    public async Task<List<Invite>> DeleteInvitesByWorkspaceIdAsync(int workspaceId)
    {
        var invites = await _context.Invite.Where(i => i.WorkspaceId == workspaceId).ToListAsync();
        
        _context.Invite.RemoveRange(invites);
        await _context.SaveChangesAsync();
        return invites;
    }
    
    //Pending invite exists 
    // Check if an invite exists and is pending
    public async Task<bool> PendingInviteExistsAsync(string inviterId, string inviteeId, int workspaceId)
    {
        return await _context.Invite.AnyAsync(i =>
            i.InviterId == inviterId && i.InviteeId == inviteeId && i.WorkspaceId == workspaceId && i.InviteStatus == "Pending");
    }

}