using Application.Dtos.WorkspaceActivityDto;
using Domain.Interfaces;

namespace Application.Services.WorkspaceActivity;

public class WorkspaceActivityService : IWorkspaceActivityService
{
    private readonly IWorkspaceActivityRepository _workspaceActivityRepo;

    public WorkspaceActivityService(IWorkspaceActivityRepository workspaceActivityRepo)
    {
        _workspaceActivityRepo = workspaceActivityRepo;
    }

    public Task<List<WorkspaceActivityDto>> GetActivityByWorkspaceId(int workspaceId)
    {
        throw new NotImplementedException();
    }
}