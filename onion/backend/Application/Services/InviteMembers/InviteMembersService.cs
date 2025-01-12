using Application.Dtos.InviteDtos;
using Application.Dtos.MembersDtos;
using Application.Handlers.Invite;
using Application.Handlers.Members;
using Application.Services.Authorization;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services.InviteMembers;

public class InviteMembersService: IInviteMembersService
{

    private readonly IInviteRepository _inviteRepository;
    private readonly IMembersRepository _membersRepository;
    private readonly IInviteDeleteHandler _inviteDeleteHandler;
    private readonly IMembersDeleteHandler _membersDeleteHandler;
    private readonly UserContext _userContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepository;

    public InviteMembersService(IInviteRepository inviteRepository, IMembersRepository membersRepository,
        IInviteDeleteHandler inviteDeleteHandler, IMembersDeleteHandler membersDeleteHandler,
        UserContext userContext, IAuthorizationService authorizationService, IWorkspaceActivityRepository workspaceActivityRepository)
    {
        _inviteRepository = inviteRepository;
        _membersRepository = membersRepository;
        _inviteDeleteHandler = inviteDeleteHandler;
        _membersDeleteHandler = membersDeleteHandler;
        _userContext = userContext;
        _authorizationService = authorizationService;
        _workspaceActivityRepository = workspaceActivityRepository;
    }
    
    public async Task<List<InviteInfoDto>> GetAllInvites()
    {
        var invites = await _inviteRepository.GetInvites();
        var invitesDto = new List<InviteInfoDto>();
        foreach (var invite in invites)
        {
            invitesDto.Add(new InviteInfoDto(invite));
        }

        return invitesDto;
    }

    public async Task<List<InviteInfoDto>> GetInvitesByWorkspace(int workspaceId)
    {
        if (!await _authorizationService.OwnsWorkspace(_userContext.Id, workspaceId ))
            throw new Exception("You are not authorized");

        var invites = await _inviteRepository.GetInvites(workspaceId: workspaceId);
        var invitesDtos = new List<InviteInfoDto>();
        foreach (var invite in invites)
        {
            invitesDtos.Add(new InviteInfoDto(invite));
        }

        return invitesDtos;
    }

    public async Task<bool> CheckPendingInvite(CreateInviteDto createInviteDto)
    {
        if (!await _authorizationService.IsMember(_userContext.Id, createInviteDto.WorkspaceId))
            throw new Exception("You are not authorized");

        var invites = await _inviteRepository.GetInvites(inviterId: createInviteDto.InviterId,
            inviteeId: createInviteDto.InviteeId, workspaceId: createInviteDto.WorkspaceId);
        var invite = invites.FirstOrDefault();
        if (invite == null)
            return false;
        if (invite.InviteStatus != "Pending")
            return false;
        return true;
    }

    public async Task<InviteInfoDto> Invite(CreateInviteDto createInviteDto)
    {
        if (!await _authorizationService.IsMember(_userContext.Id, createInviteDto.WorkspaceId))
            throw new Exception("You are not authorized");

        var newInvite = new Domain.Entities.Invite(
            createInviteDto.WorkspaceId,
            createInviteDto.InviterId,
            createInviteDto.InviteeId,
            "Pending");
        var invite = await _inviteRepository.CreateInvite(newInvite);

        var inviteNew = (await _inviteRepository.GetInvites(invite.InviteId)).FirstOrDefault();
        var newActivity = new Domain.Entities.WorkspaceActivity(createInviteDto.WorkspaceId,
            _userContext.Id,
            "Invited",
            inviteNew.Invitee.FirstName+" "+inviteNew.Invitee.LastName,
            DateTime.Now);
        await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);

        return new InviteInfoDto(newInvite);
    }

    public async Task<InviteInfoDto> UpdateInviteStatus(UpdateInviteDto updateInviteDto)
    {
        
        
        var invite = (await _inviteRepository.GetInvites(updateInviteDto.InviteId)).FirstOrDefault();
        
        if (invite == null)
            throw new Exception("Invite not found");
        
        if (_userContext.Id != invite.InviteeId)
            throw new Exception("You are not authorized");
        
        
        invite.InviteId = updateInviteDto.InviteId;
        invite.InviteStatus = updateInviteDto.InviteStatus;

        var updatedInvite = await _inviteRepository.UpdateInvite(invite);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(invite.WorkspaceId,
            _userContext.Id,
            updateInviteDto.InviteStatus,
            "the invite",
            DateTime.Now);
        await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);

        var member = new Members(
            updatedInvite.InviteeId, 
            updatedInvite.WorkspaceId, 
            DateTime.Now
            );
        if (updatedInvite.InviteStatus == "Accepted")
        {
           await _membersRepository.CreateMember(member);
        }

        return new InviteInfoDto(updatedInvite);
    }

    public async Task<InviteInfoDto> UpdateInvite(UpdateInviteAdminDto updateInviteAdminDto)
    {
        if (!await _authorizationService.IsAdmin(_userContext.Id))
            throw new Exception("You are not authorized");

        var invite = (await _inviteRepository.GetInvites(updateInviteAdminDto.InviteId)).FirstOrDefault();
      
        if (invite == null)
            throw new Exception("Invite not found");

        invite.InviteId = updateInviteAdminDto.InviteId;
        invite.InviterId = updateInviteAdminDto.InviterId;
        invite.InviteeId = updateInviteAdminDto.InviteeId;
        invite.InviteStatus = updateInviteAdminDto.InviteStatus;
        
       var updatedInvite = await _inviteRepository.UpdateInvite(invite);
        return new InviteInfoDto(updatedInvite);
    }

    public async Task<InviteInfoDto> DeleteInviteById(InviteIdDto inviteIdDto)
    {
        var invite = (await _inviteRepository.GetInvites(inviteId: inviteIdDto.InviteId)).FirstOrDefault();
        if (invite == null)
            throw new Exception("Invite not found");

        
        if (!await _authorizationService.IsMember(_userContext.Id, invite.WorkspaceId ))
            throw new Exception("You are not authorized");

        await _inviteDeleteHandler.HandleDeleteRequest(invite.InviteId);
        
        return new InviteInfoDto(invite);
    }

    public async Task<List<MemberDto>> GetAllMembers()
    {
        var members = await _membersRepository.GetMembers();
        var membersDto = new List<MemberDto>();
        foreach (var member in members)
        {
            membersDto.Add(new MemberDto(member));
        }

        return membersDto;
    }

    public async Task<List<MemberDto>> GetAllMembersByWorkspace(int workspaceId)
    {
        if (!await _authorizationService.IsMember(_userContext.Id, workspaceId ))
            throw new Exception("You are not authorized");

        var members = await _membersRepository.GetMembers(workspaceId: workspaceId);
        var membersDto = new List<MemberDto>();
        foreach (var member in members)
        {
            membersDto.Add(new MemberDto(member));
        }

        return membersDto;
    }

    public async Task<MemberDto> UpdateMember(UpdateMemberDto updateMemberDto)
    {
        if (!await _authorizationService.IsAdmin(_userContext.Id))
            throw new Exception("You are not authorized");

        var member = (await _membersRepository.GetMembers(memberId: updateMemberDto.MemberId)).FirstOrDefault();
   
        if (member == null)
            throw new Exception("Member not found");
        
        member.WorkspaceId = updateMemberDto.WorkspaceId;
        member.UserId = updateMemberDto.UserId;
        
        var updateMember = await _membersRepository.UpdateMember(member);
        return new MemberDto(updateMember);
    }

    public async Task<MemberDto> RemoveMember(RemoveMemberDto removeMemberDto)
    {
        if (!await _authorizationService.IsMember(_userContext.Id, removeMemberDto.WorkspaceId))
            throw new Exception("You are not authorized");

        if (await _authorizationService.OwnsWorkspace(removeMemberDto.UserId, removeMemberDto.WorkspaceId))
            throw new Exception("You cannot remove the owner");
        
        var member = (await _membersRepository.GetMembers(memberId: removeMemberDto.WorkspaceId)).FirstOrDefault();
        if (member == null)
            throw new Exception("Member not found");

        
        //kjo krijon aktivitetin
        await _membersDeleteHandler.HandleDeleteRequest(member.MemberId);
        
        return new MemberDto(member);
    }

    public async Task<MemberDto> DeleteMember(MemberIdDto memberIdDto)
    {
        if (!await _authorizationService.IsAdmin(_userContext.Id))
            throw new Exception("You are not authorized");

        var member = (await _membersRepository.GetMembers(memberId: memberIdDto.MemberId)).FirstOrDefault();
        if (member == null)
            throw new Exception("Member not found");

        await _membersDeleteHandler.HandleDeleteRequest(member.MemberId);
        
        return new MemberDto(member);
    }
}