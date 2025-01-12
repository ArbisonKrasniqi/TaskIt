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

    public async Task<List<WorkspaceActivityDto>> GetActivityByWorkspaceId(int workspaceId)
    {
        var workspaceActivities = await _workspaceActivityRepo.GetWorkspaceActivity(workspaceId: workspaceId);

        var workspaceActivityDtos = new List<WorkspaceActivityDto>();
        foreach (var workspaceActivity in workspaceActivities)
        {
            workspaceActivityDtos.Add(new WorkspaceActivityDto(workspaceActivity));
        }

        return workspaceActivityDtos;
    }
}