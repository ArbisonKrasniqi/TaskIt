using Domain.Interfaces;

namespace Application.Handlers.WorkspaceActivity;

public class WorkspaceActivityDeleteHandler : IWorkspaceActivityDeleteHandler
{
    private readonly IWorkspaceActivityRepository _workspaceActivityRepo;

    public WorkspaceActivityDeleteHandler(
        IWorkspaceActivityRepository workspaceActivityRepo)
    {
        _workspaceActivityRepo = workspaceActivityRepo;
    }

    public async Task HandleDeleteRequest(int workspaceActivityId)
    {
        await _workspaceActivityRepo.DeleteWorkspaceActivity(workspaceActivityId);
    }
}