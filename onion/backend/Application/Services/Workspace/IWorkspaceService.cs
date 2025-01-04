using Application.Dtos.WorkspaceDtos;

namespace Application.Services.Workspace;

public interface IWorkspaceService
{
    Task<List<WorkspaceDto>> GetAllWorkspaces();
    Task<List<WorkspaceDto>> GetWorkspacesByMemberId(string memberId);
    Task<WorkspaceDto> GetWorkspaceById(int workspaceId);
    Task<WorkspaceDto> CreateWorkspace(CreateWorkspaceDto createWorkspaceDto);
    Task<WorkspaceDto> UpdateWorkspace(UpdateWorkspaceDto updateWorkspaceDto);
    Task<WorkspaceDto> DeleteWorkspace(WorkspaceIdDto workspaceIdDto);
}