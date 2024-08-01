namespace backend.Mappers.Member;
using AutoMapper;
using Models;
using DTOs.Members.Output;

public class MemberProfile : Profile
{
    public MemberProfile()
    {
        CreateMap<Members, MemberDto>();
    }
}