using Application.Dtos.WorkspaceDtos;
using Application.Handlers.Workspace;
using Application.Services.Authorization;
using Domain.Interfaces;
namespace Application.Services.Workspace;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IWorkspaceDeleteHandler _workspaceDeleteHandler;
    private readonly UserContext _userContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepository;
    private readonly IMembersRepository _membersRepository;
    public WorkspaceService(IWorkspaceRepository workspaceRepository, IWorkspaceDeleteHandler workspaceDeleteHandler,
        UserContext userContext, IAuthorizationService authorizationService, IWorkspaceActivityRepository workspaceActivityRepository, IMembersRepository membersRepository)
    {
        _workspaceRepository = workspaceRepository;
        _workspaceDeleteHandler = workspaceDeleteHandler;
        _userContext = userContext;
        _authorizationService = authorizationService;
        _workspaceActivityRepository = workspaceActivityRepository;
        _membersRepository = membersRepository;
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
            DateTime.Now,
            _userContext.Id
        );
        var workspace = await _workspaceRepository.CreateWorkspace(newWorkspace); 
        
        var ownerMember = new Domain.Entities.Members
        {
            UserId = workspace.OwnerId,
            DateJoined = DateTime.Now,
            WorkspaceId = workspace.WorkspaceId
        };
        workspace.Members = new List<Domain.Entities.Members> { ownerMember };

       await _membersRepository.CreateMember(ownerMember);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(workspace.WorkspaceId,
        _userContext.Id,
        "Created",
        workspace.Title,
        DateTime.Now);
        await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);
        
        return new WorkspaceDto(workspace);
    }

    public async Task<WorkspaceDto> UpdateWorkspace(UpdateWorkspaceDto updateWorkspaceDto)
    {
        var isMember = await _authorizationService.IsMember(_userContext.Id, updateWorkspaceDto.WorkspaceId);
        if (!isMember && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var workspace = (await _workspaceRepository.GetWorkspaces(updateWorkspaceDto.WorkspaceId)).FirstOrDefault();
      
        if (workspace == null)
            throw new Exception("Workspace not found");

        workspace.Title = updateWorkspaceDto.Title;
        workspace.Description = updateWorkspaceDto.Description;
        workspace.OwnerId = updateWorkspaceDto.OwnerId;
        
        var updatedWorkspace = await _workspaceRepository.UpdateWorkspace(workspace);

        var newActivity = new Domain.Entities.WorkspaceActivity(workspace.WorkspaceId,
            _userContext.Id,
            "Updated",
            workspace.Title,
            DateTime.Now);
        await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);

        return new WorkspaceDto(updatedWorkspace);

    }

    public async Task<WorkspaceDto> DeleteWorkspace(WorkspaceIdDto workspaceIdDto)
    {
        var accessesWorkspace = await _authorizationService.OwnsWorkspace(_userContext.Id, workspaceIdDto.WorkspaceId);
        if (!accessesWorkspace && _userContext.Role != "Admin") throw new Exception("You are not authorized");
        
        var workspace = (await _workspaceRepository.GetWorkspaces(workspaceId: workspaceIdDto.WorkspaceId)).FirstOrDefault();
        if (workspace == null)
            throw new Exception("Workspace not found");

        await _workspaceDeleteHandler.HandleDeleteRequest(workspace.WorkspaceId);
        
        return new WorkspaceDto(workspace);
    }
}