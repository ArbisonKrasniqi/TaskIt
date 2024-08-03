using AutoMapper;
using AutoMapper.Execution;
using backend.DTOs.Board.Output;
using backend.DTOs.Members.Output;
using backend.DTOs.Workspace;

namespace backend.Mappers.Workspace;

public class WorkspaceProfile : Profile
{
    public WorkspaceProfile()
    {
        CreateMap<Models.Workspace, WorkspaceDto>()
            .ForMember(dest => dest.Boards,
                opt => opt.MapFrom(src => src.Boards))
            .ForMember(dest => dest.Members,
                opt => opt.MapFrom(src => src.Members));

        CreateMap<CreateWorkspaceRequestDto, Models.Workspace>();
        CreateMap<UpdateWorkspaceRequestDto, Models.Workspace>();
        CreateMap<Models.Members, MemberDto>();
        CreateMap<Models.Board, BoardDto>();
    }
}
