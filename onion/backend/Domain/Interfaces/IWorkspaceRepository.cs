using Domain.Entities;

namespace Domain.Interfaces;

public interface IWorkspaceRepository
{
    Task<IEnumerable<Workspace>> GetWorkspaces(
        int? workspaceId = null,
        string ownerId = null,
        string memberId = null
    );

    Task<Workspace> CreateWorkspace(Workspace workspace);
    Task<Workspace> UpdateWorkspace(Workspace workspace);
    Task<Workspace> DeleteWorkspace(int workspaceId);
}