using Application.Dtos.InviteDtos;
using Application.Dtos.MembersDtos;

namespace Application.Services.InviteMembers;

public interface IInviteMembersService
{
    //Invite
    Task<List<InviteInfoDto>> GetAllInvites();
    Task<List<InviteInfoDto>> GetInvitesByWorkspace(int workspaceId);
    Task<bool> CheckPendingInvite(CreateInviteDto createInviteDto);
    Task<InviteInfoDto> Invite(CreateInviteDto createInviteDto);
    Task<InviteInfoDto> UpdateInviteStatus(UpdateInviteDto updateInviteDto);
    Task<InviteInfoDto> UpdateInvite(UpdateInviteAdminDto updateInviteAdminDto);
    Task<InviteInfoDto> DeleteInviteById(InviteIdDto inviteIdDto);
    //Members
    Task<List<MemberDto>> GetAllMembers();
    Task<List<MemberDto>> GetAllMembersByWorkspace(int workspaceId);
    Task<MemberDto> RemoveMember(RemoveMemberDto removeMemberDto);
    Task<MemberDto> DeleteMember(MemberIdDto memberIdDto);

}