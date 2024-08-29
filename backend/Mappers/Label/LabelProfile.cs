
using AutoMapper;
using backend.DTOs.Label;
using backend.DTOs.Task;

namespace backend.Mappers.Label;

public class LabelProfile : Profile{

    public LabelProfile(){
        CreateMap<Models.Label, LabelDto>();
        CreateMap<CreateLabelRequestDTO, Models.Label>();
        CreateMap<UpdateLabelRequestDTO, Models.Label>();

    }
}

