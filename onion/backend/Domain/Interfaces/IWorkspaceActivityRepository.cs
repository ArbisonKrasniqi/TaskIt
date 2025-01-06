using Domain.Entities;

namespace Domain.Interfaces;

public interface IWorkspaceActivityRepository
{
    Task<IEnumerable<WorkspaceActivity>> GetWorkspaceActivity(
        int? workspaceActivityId = null,
        int? workspaceId = null,
        int? userId = null
    );
    Task<WorkspaceActivity> CreateWorkspaceActivity(WorkspaceActivity workspaceActivity);
    Task<WorkspaceActivity> UpdateWorkspaceActivity(WorkspaceActivity workspaceActivity);
    Task<WorkspaceActivity> DeleteWorkspaceActivity(int workspaceActivityId);
}