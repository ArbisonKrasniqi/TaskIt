using AutoMapper;
using backend.DTOs.List;

namespace backend.Mappers.Lista;

public class ListProfile : Profile 
{
    public ListProfile()
    {
        CreateMap<Models.List, ListDTO>().ForMember(dest => dest.Tasks,
            opt => opt.MapFrom(src => src.Tasks));

        CreateMap<CreateListDTO, Models.List>();
        CreateMap<UpdateListDTO, Models.List>();
    }
}