using AutoMapper;
using backend.DTOs.TaskMember.Output;

namespace backend.Mappers.TaskMember;

public class TaskMemberProfile : Profile
{
    public TaskMemberProfile()
    {
        CreateMap<Models.TaskMember, TaskMemberDto>();
    }
}