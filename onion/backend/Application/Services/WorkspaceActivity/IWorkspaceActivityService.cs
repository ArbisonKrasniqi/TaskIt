using Application.Dtos.WorkspaceActivityDto;

namespace Application.Services.WorkspaceActivity;

public interface IWorkspaceActivityService
{
    Task<List<WorkspaceActivityDto>> GetActivityByWorkspaceId(int workspaceId);
}