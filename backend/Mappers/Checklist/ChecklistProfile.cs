using AutoMapper;
using backend.DTOs.Checklist.Input;
using backend.DTOs.Checklist.Output;

namespace backend.Mappers.Checklist;

public class ChecklistProfile : Profile
{
    public ChecklistProfile()
    {
        CreateMap<ChecklistDTO, Models.Checklist>();
        
        CreateMap<CreateChecklistDTO, Models.Checklist>();
        CreateMap<UpdateChecklistDTO, Models.Checklist>();
    }
    
}