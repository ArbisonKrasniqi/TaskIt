using backend.DTOs.Workspace;
using backend.Models;

namespace backend.Interfaces;

public interface IWorkspaceRepository
{
    Task<List<Workspace>> GetAllWorkspacesAsync();
    Task<List<Workspace>> GetWorkspacesByOwnerIdAsync(string userId);
    Task<Workspace?> GetWorkspaceByIdAsync(int id); //? sepse e perdorim FirstOrDefault dhe mundet me kthy null
    Task<Workspace> CreateWorkspaceAsync(Workspace workspaceModel);
    Task<Workspace?> UpdateWorkspaceAsync(int id, UpdateWorkspaceRequestDto workspaceDto);
    Task<Workspace?> DeleteWorkspaceAsync(int id);
    Task<List<Workspace>> DeleteWorkspacesByOwnerIdAsync(string id);
}