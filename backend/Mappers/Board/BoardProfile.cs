using AutoMapper;
using backend.DTOs.Board.Input;
using backend.DTOs.Board.Output;
using backend.Models;

namespace backend.Mappers.Board;

public class BoardProfile : Profile
{
    public BoardProfile()
    {
        CreateMap<Models.Board, BoardDto>().ForMember(dest => dest.Lists,
            opt => opt.MapFrom(src => src.Lists));

        CreateMap<CreateBoardDto, Models.Board>();
        CreateMap<UpdateBoardRequestDto, Models.Board>();
    }
}