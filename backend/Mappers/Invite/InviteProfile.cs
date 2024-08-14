using AutoMapper;
using backend.DTOs.Invite.Input;
using backend.DTOs.Invite.Output;

namespace backend.Mappers.Invite;

public class InviteProfile : Profile
{
    public InviteProfile()
    {
        CreateMap<Models.Invite, InviteDtoOut>();
        CreateMap<UpdateInviteDto, Models.Invite>();
        CreateMap<InviteDto, Models.Invite>();
        CreateMap<Models.Invite, InviteDto>();
    }
}