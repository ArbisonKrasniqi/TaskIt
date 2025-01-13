using Domain.Interfaces;

namespace Application.Handlers.WorkspaceActivity;

public class WorkspaceActivityDeleteHandler : IWorkspaceActivityDeleteHandler
{
    private readonly IWorkspaceActivityRepository _workspaceActivityRepo;
    private readonly UserContext _userContext;

    public WorkspaceActivityDeleteHandler(
        IWorkspaceActivityRepository workspaceActivityRepo, UserContext userContext)
    {
        _workspaceActivityRepo = workspaceActivityRepo;
        _userContext = userContext;
    }

    public async Task HandleDeleteRequest(int workspaceActivityId)
    {
        var workspaceActivity =
            (await _workspaceActivityRepo.GetWorkspaceActivity(workspaceActivityId: workspaceActivityId))
            .FirstOrDefault();
        await _workspaceActivityRepo.DeleteWorkspaceActivity(workspaceActivityId);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(workspaceActivity.Workspace.WorkspaceId,
            _userContext.Id,
            "Deleted",
            workspaceActivity.EntityName,
            DateTime.Now);
        await _workspaceActivityRepo.CreateWorkspaceActivity(newActivity);
    }
}