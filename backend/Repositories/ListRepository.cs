using backend.Data;
using backend.DTOs.List;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ListRepository : IListRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IListRepository _listRepo;
    public ListRepository(ApplicationDBContext context, IListRepository listRepo)
    {
        _listRepo = listRepo;
        _context = context;
    }

    public async Task<List<List>> GetAllListsAsync()
    {
        return await _context.Lists.ToListAsync();
    }

    public async Task<List?> GetListByIdAsync(int ListId)
    {
        return await _context.Lists.FindAsync(ListId);
    }

    public async Task<List?> CreateListAsync(List listModel)
    {
        await _context.Lists.AddAsync(listModel);
        await _context.SaveChangesAsync();
        return listModel;
    }

    public async Task<List?> UpdateListAsync(int ListId, UpdateListDTO listDto)
    {
        var existingList = await _context.Lists.FirstOrDefaultAsync(x => x.ListId == ListId);

        if (existingList == null)
        {
            return null;
        }
        
        existingList.Title = listDto.Title;
        existingList.DateCreated = listDto.DateCreated;

        await _context.SaveChangesAsync();
        return existingList;
    }

    public async Task<List?> DeleteListAsync(int ListId)
    {
        var listModel = await _context.Lists.FirstOrDefaultAsync(x => x.ListId == ListId);

        if (listModel == null)
        {
            return null;
        }

        _context.Lists.Remove(listModel);
        await _context.SaveChangesAsync();
        return listModel;
    }
}