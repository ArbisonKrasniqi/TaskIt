using backend.DTOs.WorkspaceActivity.Output;
using backend.Models;

namespace backend.Interfaces;

public interface IWorkspaceActivityRepository
{
 Task<WorkspaceActivity> CreateWorkspaceActivityAsync(WorkspaceActivity workspaceActivity);
 Task<List<WorkspaceActivityDto>> GetAllWorkspacesActivityAsync();
 Task<WorkspaceActivityDto> GetWorkspaceActivityByIdAsync(int workspaceActivityId);
 Task<List<WorkspaceActivityDto>> GetWorkspaceActivitiesByWorkspace(int workspaceId);
 Task<List<WorkspaceActivity?>> DeleteWorkspaceActivitiesByWorkspace(int workspaceId);
 Task<WorkspaceActivity?> DeleteWorkspaceActivityByIdAsync(int workspaceActivityId);
}