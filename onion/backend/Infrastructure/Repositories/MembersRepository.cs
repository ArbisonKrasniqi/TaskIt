using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MembersRepository : IMembersRepository
{
    private readonly AppDbContext _context;

    public MembersRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Members>> GetMembers(int? memberId = null,
        string userId = null, int? workspaceId = null, DateTime? dateJoined = null)
    {
        var query = _context.Members.Include(m=>m.Workspace).Include(m=>m.User).AsQueryable();

        if (memberId.HasValue)
            query = query.Where(m => m.MemberId == memberId);
        if (!string.IsNullOrEmpty(userId))
            query = query.Where(m => m.UserId == userId);
        if (workspaceId.HasValue)
            query = query.Where(m => m.WorkspaceId == workspaceId);
        if (dateJoined.HasValue)
            query = query.Where(m => m.DateJoined == dateJoined);

        return await query.ToListAsync();
    }

    public async Task<Members> CreateMember(Members member)
    {
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task<Members> UpdateMember(Members member)
    {
        var existingMember = await _context.Members.FindAsync(member.MemberId);
        if (existingMember == null)
            throw new Exception("Member not found");
        
        _context.Entry(existingMember).CurrentValues.SetValues(member);
        await _context.SaveChangesAsync();
        return existingMember;
    }

    public async Task<Members> DeleteMember(int memberId)
    {
        var member = await _context.Members.FindAsync(memberId);

        if (member == null)
            throw new Exception("Member not found");

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();

        return member;
    }
}