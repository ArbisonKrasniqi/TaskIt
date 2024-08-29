using AutoMapper;
using backend.DTOs.ChecklistItem.Input;
using backend.DTOs.ChecklistItem.Output;
using Microsoft.EntityFrameworkCore;

namespace backend.Mappers.ChecklistItem;

public class ChecklistItemProfile: Profile
{
    public ChecklistItemProfile()
    {
        CreateMap<Models.ChecklistItem,ChecklistItemDTO>();
        
        CreateMap<CreateChecklistItemDTO, Models.ChecklistItem>();
        CreateMap<UpdateChecklistItemDTO, Models.ChecklistItem>();
    }
}