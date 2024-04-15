using backend.Data;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly ApplicationDBContext _context;

//constructor
    public WorkspaceRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    /*Task a wrapper for an object duhet gjithmone me e kthy nje Task kur perdorim async
     kur e perdorim async duhet me perdor await ne rreshtat ku dojna me bo async code
     ne rreshtat ku perdoret diqka prej databaze */

    //CREATE
    public async Task<Workspace> CreateWorkspaceAsync(Workspace workspaceModel)
    {
        await _context.Workspace.AddAsync(workspaceModel);
        await _context.SaveChangesAsync(); //per me i rujt ndryshimet ne bazen e te dhenave
        return workspaceModel;
    }
    //DELETE
    public async Task<Workspace?> DeleteWorkspaceAsync(int id)
    {
        var workspaceModel = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceId == id); // finds the Workspace with the given id
        if (workspaceModel == null)
        {
            return null;
        }
        _context.Workspace.Remove(workspaceModel); // te remove nuk perdorim await sepse nuk osht funksion async sepse sosht i nderlikuar
        await _context.SaveChangesAsync();
        return workspaceModel;
    }
    //GETALL
    public async Task<List<Workspace>> GetAllWorkspacesAsync()
    {
        return await _context.Workspace.Include(w=> w.Boards).ToListAsync(); 
    }
    
    //GETBYID
    public async Task<Workspace?> GetWorkspaceByIdAsync(int id)
    {
        return await _context.Workspace.Include(w => w.Boards).FirstOrDefaultAsync(i => i.WorkspaceId == id);
    }

    //UPDATE
    public async Task<Workspace?> UpdateWorkspaceAsync(int id, UpdateWorkspaceRequestDto workspaceDto)
    {
        var existingWorkspace = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceId == id);
        if (existingWorkspace == null)
        {
            return null;
        }

        existingWorkspace.Title = workspaceDto.Title;
        existingWorkspace.UserId = workspaceDto.UserId;
        existingWorkspace.Description = workspaceDto.Description;
        await _context.SaveChangesAsync();
        return existingWorkspace;
    }
    
    //EXISTS
    public async Task<bool> WorkspaceExists(int id)
    {
        return await _context.Workspace.AnyAsync(w => w.WorkspaceId == id);
    }
}

