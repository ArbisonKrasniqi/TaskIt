using Application.Handlers.Invite;
using Application.Handlers.Members;
using Domain.Interfaces;

namespace Application.Handlers.Workspace;

public class WorkspaceDeleteHandler : IWorkspaceDeleteHandler
{
    private readonly IWorkspaceRepository _workspaceRepository;
    // private readonly IBoardDeleteHandler _boardDeleteHandler;
    //private readonly IWorkspaceActivityDeleteHandler _workspaceActivityDeleteHandler;
    private readonly IMembersDeleteHandler _membersDeleteHandler;
    private readonly IInviteDeleteHandler _inviteDeleteHandler;

    public WorkspaceDeleteHandler(IWorkspaceRepository workspaceRepository,IMembersDeleteHandler membersDeleteHandler, IInviteDeleteHandler inviteDeleteHandler
    /* IBoardDeleteHandler boardDeleteHandler, IWorkspaceActivityDeleteHandler workspaceActivityDeleteHandler*/)
    {
        _workspaceRepository = workspaceRepository;
        _membersDeleteHandler = membersDeleteHandler;
        _inviteDeleteHandler = inviteDeleteHandler;
        //_boardDeleteHandler = boardDeleteHandler;
        //_workspaceActivityDeleteHandler = workspaceActivityDeleteHandler;
    }
    
    public async Task HandleDeleteRequest(int workspaceId)
    {
        var boards = (await _workspaceRepository.GetWorkspaces(workspaceId: workspaceId)).FirstOrDefault().Boards;
        if (boards.Any())
        {
            // foreach (var board in boards){
            //     await _boardDeleteHandler.HandleDeleteRequest(board.boardId);
            // }
        }

        var invites = (await _workspaceRepository.GetWorkspaces(workspaceId: workspaceId)).FirstOrDefault().Invites;
        if (invites.Any())
        {
            foreach (var invite in invites)
            {
                await _inviteDeleteHandler.HandleDeleteRequest(invite.InviteId);
            }
        }

        var members = (await _workspaceRepository.GetWorkspaces(workspaceId: workspaceId)).FirstOrDefault().Members;
        if (members.Any())
        {
            foreach (var member in members)
            {
                await _membersDeleteHandler.HandleDeleteRequest(member.MemberId);
            }
        }

        var workspaceActivity = (await _workspaceRepository.GetWorkspaces(workspaceId: workspaceId)).FirstOrDefault().Activity;
        if (workspaceActivity.Any())
        {
            // foreach (var activity in workspaceActivity)
            // {
            //     await _workspaceActivityDeleteHandler.HandleDeleteRequest(activity.WorkspaceActivityId);
            // }
        }

        await _workspaceRepository.DeleteWorkspace(workspaceId);
    }
}