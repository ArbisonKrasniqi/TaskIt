using Application.Dtos.InviteDtos;
using Application.Dtos.MembersDtos;
using Domain.Interfaces;

namespace Application.Services.InviteMembers;

public class InviteMembersService: IInviteMembesService
{

    private readonly IInviteRepository _inviteRepository;
    private readonly IMembersRepository _membersRepository;

    public InviteMembersService(IInviteRepository inviteRepository, IMembersRepository membersRepository)
    {
        _inviteRepository = inviteRepository;
        _membersRepository = membersRepository;
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
        var invites = await _inviteRepository.GetInvites(inviterId: createInviteDto.InviterId,
            inviteeId: createInviteDto.InviteeId, workspaceId: createInviteDto.WorkspaceId);
        var invite = invites.FirstOrDefault();
        if (invite == null)
            return false;
        return true;
    }

    public async Task<InviteInfoDto> Invite(CreateInviteDto createInviteDto)
    {
        var newInvite = new Domain.Entities.Invite(
            createInviteDto.WorkspaceId,
            createInviteDto.InviterId,
            createInviteDto.InviteeId);
        var invite = await _inviteRepository.CreateInvite(newInvite);
        return new InviteInfoDto(newInvite);
    }

    public async Task<InviteInfoDto> UpdateInviteStatus(UpdateInviteDto updateInviteDto)
    {
        var invites = await _inviteRepository.GetInvites(updateInviteDto.InviteId);
        var invite = invites.FirstOrDefault();
        if (invite == null)
            throw new Exception("Invite not found");
        _inviteRepository.UpdateInvite(invite);
        return new InviteInfoDto(invite);
    }

    public async Task<InviteInfoDto> UpdateInvite(UpdateInviteAdminDto updateInviteAdminDto)
    {
        var invites = await _inviteRepository.GetInvites(updateInviteAdminDto.InviteId);
        var invite = invites.FirstOrDefault();
        if (invite == null)
            throw new Exception("Invite not found");
        _inviteRepository.UpdateInvite(invite);
        return new InviteInfoDto(invite);
    }

    public async Task<InviteInfoDto> DeleteInviteById(InviteIdDto inviteIdDto)
    {
        var invites = await _inviteRepository.GetInvites(inviteId: inviteIdDto.InviteId);
        var invite = invites.FirstOrDefault();
        if (invite == null)
            throw new Exception("Invite not found");

        _inviteRepository.DeleteInvite(inviteIdDto.InviteId);
        
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
        var members = await _membersRepository.GetMembers(memberId: updateMemberDto.MemberId);
        var member = members.FirstOrDefault();
        if (member == null)
            throw new Exception("Member not found");
        _membersRepository.UpdateMember(member);
        return new MemberDto(member);
    }

    public Task<MemberDto> RemoveMember(RemoveMemberDto removeMemberDto)
    {
        throw new NotImplementedException();
    }

    public async Task<MemberDto> DeleteMember(MemberIdDto memberIdDto)
    {
        var members = await _membersRepository.GetMembers(memberId: memberIdDto.MemberId);
        var member = members.FirstOrDefault();
        if (member == null)
            throw new Exception("Member not found");
        _membersRepository.DeleteMember(memberIdDto.MemberId);
        return new MemberDto(member);
    }
}