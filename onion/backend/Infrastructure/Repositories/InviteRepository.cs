using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InviteRepository : IInviteRepository
{
    private readonly AppDbContext _context;

    public InviteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Invite>> GetInvites(int? inviteId = null,
        int? workspaceId = null, string inviterId = null, string inviteeId = null,
        string inviteStatus = null)
    {
        var query = _context.Invites.AsQueryable();
        if (inviteId.HasValue)
            query = query.Where(i => i.InviteId==inviteId);
        if (workspaceId.HasValue)
            query = query.Where(i => i.WorkspaceId == workspaceId);
        if (!string.IsNullOrEmpty(inviterId))
            query = query.Where(i => i.InviterId == inviterId);
        if (!string.IsNullOrEmpty(inviteeId))
            query = query.Where(i => i.InviteeId == inviteeId);
        if (!string.IsNullOrEmpty(inviteStatus))
            query = query.Where(i => i.InviteStatus == inviteStatus);

        return await query.ToListAsync();
    }

    public async Task<Invite> CreateInvite(Invite invite)
    {
        await _context.Invites.AddAsync(invite);
        await _context.SaveChangesAsync();
        return invite;
    }

    public async Task<Invite> UpdateInvite(Invite invite)
    {
        var existingInvite = await _context.Invites.FindAsync(invite.InviteId);

        if (existingInvite == null)
            throw new Exception("Invite not found");
        
        _context.Entry(existingInvite).CurrentValues.SetValues(invite);
        await _context.SaveChangesAsync();

        return existingInvite;
    }

    public async Task<Invite> DeleteInvite(int inviteId)
    {
        var invite = await _context.Invites.FindAsync(inviteId);

        if (invite == null)
            throw new Exception("Invite not found");

        _context.Invites.Remove(invite);
        await _context.SaveChangesAsync();

        return invite;
    }
}