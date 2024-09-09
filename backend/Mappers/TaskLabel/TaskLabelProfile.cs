using AutoMapper;
using backend.DTOs.TaskLabel.Output;
using backend.DTOs.TaskLabelDTO.Input;

namespace backend.Mappers.TaskLabel;

public class TaskLabelProfile : Profile
{
    public TaskLabelProfile()
    {
        CreateMap<AssignLabelDTO, Models.TaskLabel>();
        CreateMap<Models.TaskLabel, TaskLabelDTO>();
        CreateMap<List<TaskLabelDTO>, Models.TaskLabel>();
    }
}