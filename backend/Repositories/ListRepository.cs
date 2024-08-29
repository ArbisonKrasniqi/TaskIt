﻿using backend.Data;
using backend.DTOs.List;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ListRepository : IListRepository
{
    private readonly ApplicationDBContext _context;
    private readonly ITaskRepository _taskRepo;
    public ListRepository(ApplicationDBContext context, ITaskRepository taskRepo)
    {
        _context = context;
        _taskRepo = taskRepo;
    }

    public async Task<List<List>> GetAllListsAsync()
    {
        return await _context.List
            .Include(l=>l.Tasks)
            .ToListAsync();
    }

    public async Task<List?> GetListByIdAsync(int ListId)
    {
        return await _context.List
            .Include(l=>l.Tasks)
            .FirstOrDefaultAsync(t=>t.ListId==ListId);
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

        await _taskRepo.DeleteTaskByListIdAsync(ListId);
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
        return await _context.List
            .Include(l=>l.Tasks)
            .Where(b => b.BoardId == BoardId).ToListAsync();
    }
    
    public async Task<List<List>> DeleteListsByBoardIdAsync(int BoardId)
    {
        var lists = await _context.List
            .Include(l=>l.Tasks)
            .Where(l => l.BoardId == BoardId).ToListAsync();

        if (lists.Count == 0) return null;

        foreach (var list in lists)
        {
            await _taskRepo.DeleteTaskByListIdAsync(list.ListId);
        }
        
        _context.List.RemoveRange(lists);
        await _context.SaveChangesAsync();
        return lists;
    }

    public async Task<bool> ListExists(int listId)
    {
        return await _context.List.AnyAsync(i => i.ListId == listId);
    }

    public async Task<bool> ListInBoard(int listId, int boardId)
    {
        var boardModel = await _context.Board.FirstOrDefaultAsync(x => x.BoardId == boardId);
        var listModel = await _context.List.FirstOrDefaultAsync(x => x.ListId == listId);
        if (boardModel != null && listModel != null)
        {
            if (listModel.BoardId == boardId)
            {
                return true;
            }
        }

        return false;
    }
    
}