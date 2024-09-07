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

        if (listModel.index == 0)
        {
            await _taskRepo.DeleteTaskByListIdAsync(ListId);
            _context.List.Remove(listModel);
            await _context.SaveChangesAsync();
            return listModel;
        }
        
        var listsToUpdate = _context.List
            .Where(l => l.BoardId == listModel.BoardId && l.index > listModel.index)
            .OrderBy(l => l.index)
            .ToList();
            
        _context.List.Remove(listModel);
            
        foreach (var list in listsToUpdate)
        {
            list.index -= 1;
        }
        _context.SaveChanges();
        return listModel;
    }

    //
    public async Task<List> CreateAsync(List listModel)
    {
        var lists = _context.List.Where(l => l.BoardId == listModel.BoardId);
        if (lists.Count() == 0)
        {
            listModel.index = 0;
            await _context.List.AddAsync(listModel);
            await _context.SaveChangesAsync();
        }

        listModel.index = lists.Count();
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

    public async Task<bool> HandleDragNDrop(List updatedList, int newIndex)
    {
        var list = await _context.List.FirstOrDefaultAsync(l => l.ListId == updatedList.ListId);
        if (list == null) return false;

        var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardId == list.BoardId);
        if (board == null) return false;

        var lists = await _context.List
            .Where(l => l.BoardId == board.BoardId)
            .OrderBy(l => l.index)
            .ToListAsync();
        var currentIndex = list.index;
        for (int i = Math.Min(newIndex, currentIndex); i <= Math.Max(newIndex, list.index); i++)
        {
            if (lists[i] != list)
            {
                if (currentIndex < newIndex)
                {
                    lists[i].index -= 1;
                }

                if (currentIndex > newIndex)
                {
                    lists[i].index += 1;
                }
            } 
        }
        list.index = newIndex;
        await _context.SaveChangesAsync();
        return true;
    }
}