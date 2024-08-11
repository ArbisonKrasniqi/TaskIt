using backend.Data;
using backend.DTOs.Invite.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;


namespace backend.Repositories;

public class InviteRepository : IInviteRepository
{
    private readonly ApplicationDBContext _context;

    public InviteRepository(ApplicationDBContext context)
    {
        _context = context;
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
    public async Task<bool> InviteExistsAsync(string inviterId, string inviteeId, int workspaceId)
    {
        return await _context.Invite.AnyAsync(i =>
            i.InviterId == inviterId && i.InviteeId == inviteeId && i.WorkspaceId == workspaceId);
    }
    
    //AddInviteAsync
    public async Task<Invite> AddInviteAsync(Invite invite)
    {    
        var inviteExists = await InviteExistsAsync(invite.InviterId, invite.InviteeId, invite.WorkspaceId);
    
        if (inviteExists)
        {
            // Handle the case where the invite already exists, maybe return null or throw an exception.
            throw new InvalidOperationException("Invite already exists.");
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

}