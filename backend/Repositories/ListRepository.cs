using backend.Data;
using backend.DTOs.List;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ListRepository : IListRepository
{
    private readonly ApplicationDBContext _context;
    public ListRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<List>> GetAllListsAsync()
    {
        return await _context.List.ToListAsync();
    }

    public async Task<List?> GetListByIdAsync(int ListId)
    {
        return await _context.List.FindAsync(ListId);
    }

    public async Task<List?> CreateListAsync(List listModel)
    {
        await _context.List.AddAsync(listModel);
        await _context.SaveChangesAsync();
        return listModel;
    }

    public async Task<List?> UpdateListAsync(UpdateListDTO listModel)
    {
        var existingList = await _context.List.FirstOrDefaultAsync(x => x.ListId == listModel.ListId);

        if (existingList == null)
        {
            return null;
        }
        
        existingList.Title = listModel.Title;

        await _context.SaveChangesAsync();
        return existingList;
    }

    public async Task<List?> DeleteListAsync(int ListId)
    {
        var listModel = await _context.List.FirstOrDefaultAsync(x => x.ListId == ListId);

        if (listModel == null)
        {
            return null;
        }

        _context.List.Remove(listModel);
        await _context.SaveChangesAsync();
        return listModel;
    }
    
    //
    public async Task<List> CreateAsync(List listModel)
    {
        await _context.List.AddAsync(listModel);
        await _context.SaveChangesAsync();
        return listModel;
    }
    
    public async Task<List<List>> GetListByBoardId(int BoardId)
    {
        return await _context.List.Where(b => b.BoardId == BoardId).ToListAsync();
    }
    
    public async Task<List<List>> DeleteListsByBoardIdAsync(int BoardId)
    {
        var lists = await _context.List.Where(l => l.BoardId == BoardId).ToListAsync();

        if (lists.Count == 0) return null;
        
        _context.List.RemoveRange(lists);
        await _context.SaveChangesAsync();
        return lists;
    }
}