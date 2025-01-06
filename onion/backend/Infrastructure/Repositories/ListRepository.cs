using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class ListRepository : IListRepository
{
    private readonly AppDbContext _context;

    public ListRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<List>> GetLists(int? listId = null, int? index = null, int? boardId = null)
    {
        throw new NotImplementedException();
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