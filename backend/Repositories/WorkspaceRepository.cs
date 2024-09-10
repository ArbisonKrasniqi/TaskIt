using backend.Data;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IBoardRepository _boardRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly IInviteRepository _inviteRepo;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepo;
    private readonly IStarredBoardRepository _starredBoardRepo;

//constructor
    public WorkspaceRepository(ApplicationDBContext context, IBoardRepository boardRepo,IMembersRepository membersRepo, IInviteRepository inviteRepo, IWorkspaceActivityRepository workspaceActivityRepo, IStarredBoardRepository starredBoardRepo)
    {
        _context = context;
        _boardRepo = boardRepo;
        _membersRepo = membersRepo;
        _inviteRepo = inviteRepo;
        _workspaceActivityRepo = workspaceActivityRepo;
        _starredBoardRepo = starredBoardRepo;
    }
    /*Task a wrapper for an object duhet gjithmone me e kthy nje Task kur perdorim async
     kur e perdorim async duhet me perdor await ne rreshtat ku dojna me bo async code
     ne rreshtat ku perdoret diqka prej databaze */

    //CREATE
    public async Task<Workspace> CreateWorkspaceAsync(Workspace workspaceModel)
    {
            await _context.Workspace.AddAsync(workspaceModel);
            await _context.SaveChangesAsync(); 
            
            var ownerMember = new Models.Members
            {
                UserId = workspaceModel.OwnerId,
                DateJoined = DateTime.Now,
                WorkspaceId = workspaceModel.WorkspaceId
            };
            workspaceModel.Members = new List<Models.Members> { ownerMember };

            _context.Members.Add(ownerMember);
            await _context.SaveChangesAsync();   //per me i rujt ndryshimet ne bazen e te dhenave
            return workspaceModel;
    }
    
    //DELETEWorkspace
    public async Task<Workspace?> DeleteWorkspaceAsync(int id)
    {
        var workspaceModel = await _context.Workspace
            .Include(w => w.Boards)
            .Include(w => w.Members)
            .FirstOrDefaultAsync(x => x.WorkspaceId == id);

        if (workspaceModel == null)
        {
            return null;
        }
        
        
        await _boardRepo.DeleteBoardsByWorkspaceIdAsync(id);
        await _membersRepo.DeleteMembersByWorkspaceIdAsync(id);
        await _inviteRepo.DeleteInvitesByWorkspaceIdAsync(id);
        await _workspaceActivityRepo.DeleteWorkspaceActivitiesByWorkspace(id);
        await _starredBoardRepo.DeleteStarredBoardsByWorkspaceIdAsync(id);
        _context.Workspace.Remove(workspaceModel);
        await _context.SaveChangesAsync();
        return workspaceModel;
    }
    
    
    //DELETEWorkspacesBYOWNERID
    public async Task<List<Workspace?>>DeleteWorkspacesByOwnerIdAsync(string ownerId)
    {
        var workspaces = await _context.Workspace.Include(w=>w.Boards)
            .Include(w=>w.Members)
            .Where(w => w.OwnerId.Equals(ownerId)).ToListAsync();

        foreach (var workspace in workspaces)
        {
            await _boardRepo.DeleteBoardsByWorkspaceIdAsync(workspace.WorkspaceId);
            await _membersRepo.DeleteMembersByWorkspaceIdAsync(workspace.WorkspaceId);
            await _inviteRepo.DeleteInvitesByWorkspaceIdAsync(workspace.WorkspaceId);
            await _workspaceActivityRepo.DeleteWorkspaceActivitiesByWorkspace(workspace.WorkspaceId);
            await _starredBoardRepo.DeleteStarredBoardsByWorkspaceIdAsync(workspace.WorkspaceId);
        }

        _context.Workspace.RemoveRange(workspaces);
        await _context.SaveChangesAsync();
        return workspaces;

    }
    
    //GETALL
    public async Task<List<Workspace?>> GetAllWorkspacesAsync()
    {
        
        return await _context.Workspace
            .Include(w=>w.Boards)
            .Include(w => w.Members) // Përfshijmë anëtarët
            .ToListAsync();
    }
    
//GETBYOWNERID
    public async Task<List<Workspace?>> GetWorkspacesByOwnerIdAsync(string ownerId)
    {
        return await _context.Workspace
            .Include(w => w.Boards)
            .Include(w => w.Members) // Përfshijmë anëtarët
            .Where(w => w.OwnerId.Equals(ownerId))
            .ToListAsync();
    }
    
    
//GETBYMEMBERID
    public async Task<List<Workspace?>> GetWorkspacesByMemberIdAsync(string memberId)
    {
        return await _context.Workspace
            .Include(w => w.Boards)
            .Include(w => w.Members) // Përfshijmë anëtarët
            .Where(w => w.Members.Any(m => m.UserId == memberId) || w.OwnerId == memberId)
            .ToListAsync();
    }
    
    
    //GETBYID
    public async Task<Workspace?> GetWorkspaceByIdAsync(int id)
    {
       // return await _context.Workspace.Include(w => w.Boards).FirstOrDefaultAsync(i => i.WorkspaceId == id);
       return await _context.Workspace
           .Include(w=>w.Boards)
           .Include(w => w.Members) // Përfshijmë anëtarët
           .FirstOrDefaultAsync(i => i.WorkspaceId == id);
    }

    //UPDATE
    public async Task<Workspace?> UpdateWorkspaceAsync(UpdateWorkspaceRequestDto workspaceDto)
    {
        var existingWorkspace = await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceDto.WorkspaceId);
        if (existingWorkspace == null)
        {
            throw new Exception("Workspace not found");
        }

        existingWorkspace.Title = workspaceDto.Title;
        existingWorkspace.OwnerId = workspaceDto.OwnerId;
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



