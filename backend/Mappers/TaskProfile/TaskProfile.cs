using AutoMapper;
using backend.DTOs.Task;

namespace backend.Mappers.TaskProfile;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<Models.Tasks, TaskDto>();
    }
}