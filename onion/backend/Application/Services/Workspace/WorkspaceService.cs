using Application.Dtos.WorkspaceDtos;
using Application.Handlers.Workspace;
using Domain.Interfaces;
namespace Application.Services.Workspace;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IWorkspaceDeleteHandler _workspaceDeleteHandler;
    public WorkspaceService(IWorkspaceRepository workspaceRepository, IWorkspaceDeleteHandler workspaceDeleteHandler)
    {
        _workspaceRepository = workspaceRepository;
        _workspaceDeleteHandler = workspaceDeleteHandler;
    }

    public async Task<List<WorkspaceDto>> GetAllWorkspaces()
    {
        var workspaces = await _workspaceRepository.GetWorkspaces();
        var workspacesDto = new List<WorkspaceDto>();
        foreach (var workspace in workspaces)
        {
            workspacesDto.Add(new WorkspaceDto(workspace));
        }

        return workspacesDto;
    }

    public async Task<List<WorkspaceDto>> GetWorkspacesByMemberId(string memberId)
    {
        var workspaces = await _workspaceRepository.GetWorkspaces(memberId: memberId);
        var workspacesDtos = new List<WorkspaceDto>();
        foreach (var workspace in workspaces)
        {
            workspacesDtos.Add(new WorkspaceDto(workspace));
        }

        return workspacesDtos;
    }

    public async Task<WorkspaceDto> GetWorkspaceById(int workspaceId)
    {
        var workspaces = await _workspaceRepository.GetWorkspaces(workspaceId: workspaceId);
        var workspace = workspaces.FirstOrDefault();
        if (workspace == null)
            throw new Exception("Workspace not found");

        return new WorkspaceDto(workspace);
    }

    public async Task<WorkspaceDto> CreateWorkspace(CreateWorkspaceDto createWorkspaceDto)
    {
        var newWorkspace = new Domain.Entities.Workspace(
            createWorkspaceDto.Title,
            createWorkspaceDto.Description,
            DateTime.Now
            );

        var workspace = await _workspaceRepository.CreateWorkspace(newWorkspace);
        return new WorkspaceDto(newWorkspace);
    }

    public async Task<WorkspaceDto> UpdateWorkspace(UpdateWorkspaceDto updateWorkspaceDto)
    {
        var workspaces = await _workspaceRepository.GetWorkspaces(updateWorkspaceDto.WorkspaceId);
        var workspace = workspaces.FirstOrDefault();
        if (workspace == null)
            throw new Exception("Workspace not found");
        await _workspaceRepository.UpdateWorkspace(workspace);
        return new WorkspaceDto(workspace);

    }

    public async Task<WorkspaceDto> DeleteWorkspace(WorkspaceIdDto workspaceIdDto)
    {
        var workspace = (await _workspaceRepository.GetWorkspaces(workspaceId: workspaceIdDto.WorkspaceId)).FirstOrDefault();
        if (workspace == null)
            throw new Exception("Workspace not found");

        await _workspaceDeleteHandler.HandleDeleteRequest(workspace.WorkspaceId);
        
        return new WorkspaceDto(workspace);
    }
}