using backend.DTOs.Task;
using AutoMapper;


namespace backend.Mappers.Task;

public class TaskProfile : Profile{

    public TaskProfile(){
        CreateMap<Models.Tasks, TaskDto>();
        CreateMap<CreateTaskRequestDTO, Models.Tasks>();
        CreateMap<UpdateTaskRequestDTO, Models.Tasks>();
    }



   
}