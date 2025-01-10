using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ListRepository : IListRepository
{
    private readonly AppDbContext _context;

    public ListRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<List>> GetLists(int? listId = null, int? index = null, int? boardId = null)
    {
        var query = _context.Lists.AsQueryable();
        
        if(listId.HasValue)
            query = query.Where(L=>L.ListId==listId);
        if (index.HasValue)
            query = query.Where(l => l.Index == index);
        if (boardId.HasValue)
            query = query.Where(l => l.BoardId == boardId);

        return await query.ToListAsync();
    }

    public async Task<List> CreateList(List list)
    {
        await _context.Lists.AddAsync(list);
        await _context.SaveChangesAsync();
        return list;
    }

    public async Task<List> UpdateList(List list)
    {
        var existingList = await _context.Lists.FindAsync(list.ListId);

        if (existingList == null)
        {
            throw new Exception("List not found");
        }
        _context.Entry(existingList).CurrentValues.SetValues(list);
        await _context.SaveChangesAsync();
        return existingList;
    }

    public async Task<List> DeleteList(int listId)
    {
        var list = await _context.Lists.FindAsync(listId);
        if (list == null)
        {
            throw new Exception("List not foung");
        }

        _context.Lists.Remove(list);
        await _context.SaveChangesAsync();
        return list;
    }
    

}