using backend.DTOs.Workspace;
using backend.Models;

namespace backend.Interfaces;

public interface IWorkspaceRepository
{
    Task<List<Workspace>> GetAllAsync();
    Task<Workspace?> GetByIdAsync(int id); //? sepse e perdorim FirstOrDefault dhe mundet me kthy null
    Task<Workspace> CreateAsync(Workspace workspaceModel);
    Task<Workspace?> UpdateAsync(int id, UpdateWorkspaceRequestDto workspaceDto);
    Task<Workspace?> DeleteAsync(int id);
}