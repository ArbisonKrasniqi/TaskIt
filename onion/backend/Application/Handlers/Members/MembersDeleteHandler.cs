using Domain.Interfaces;

namespace Application.Handlers.Members;

public class MembersDeleteHandler : IMembersDeleteHandler
{
    private readonly IMembersRepository _membersRepository;
    private readonly UserContext _userContext;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepository;

    public MembersDeleteHandler(IMembersRepository membersRepository, UserContext userContext, IWorkspaceActivityRepository workspaceActivityRepository)
    {
        _membersRepository = membersRepository;
        _userContext = userContext;
        _workspaceActivityRepository = workspaceActivityRepository;
    }

    public async Task HandleDeleteRequest(int memberId)
    {
        var member = (await _membersRepository.GetMembers(memberId: memberId)).FirstOrDefault();
        
        await _membersRepository.DeleteMember(memberId);

        var newActivity = new Domain.Entities.WorkspaceActivity(member.WorkspaceId,
            _userContext.Id,
            "Removed",
            member.User.FirstName + " " + member.User.LastName,
            DateTime.Now);
        await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);
    }
}