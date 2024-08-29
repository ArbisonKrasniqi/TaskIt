using AutoMapper;
using backend.DTOs.User.Output;

namespace backend.Mappers.User;

public class UserProfile : Profile
{
    public UserProfile() {
//mapping prej user ne search user dto
        CreateMap<Models.User, SearchUserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

    }
}