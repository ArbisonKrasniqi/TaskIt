using AutoMapper;
using backend.DTOs.Background.Input;
using backend.DTOs.Background.Output;

namespace backend.Mappers.Background;

public class BackgroundProfile : Profile
{
    public BackgroundProfile()
    {
        CreateMap<Models.Background, BackgroundDto>();
        CreateMap<CreateBackgroundDto, Models.Background>();
        CreateMap<UpdateBackgroundDto, Models.Background>();
    }
}