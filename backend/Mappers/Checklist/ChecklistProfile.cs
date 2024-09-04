using AutoMapper;
using backend.DTOs.Checklist.Input;
using backend.DTOs.Checklist.Output;

namespace backend.Mappers.Checklist;

public class ChecklistProfile : Profile
{
    public ChecklistProfile()
    {
        CreateMap<Models.Checklist, ChecklistDTO>().ForMember(dest=> dest.ChecklistItems,
            opt=>opt.MapFrom(src=>src.ChecklistItems));
        
        CreateMap<CreateChecklistDTO, Models.Checklist>();
        CreateMap<UpdateChecklistDTO, Models.Checklist>();
        
    }
    
}