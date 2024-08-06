namespace backend.Mappers.StarredBoard;
using AutoMapper;
using Models;
using DTOs.StarredBoard;

public class StarredBoardProfile : Profile
{
    public StarredBoardProfile()
    {
        CreateMap<StarredBoard, StarredBoardDto>();
        CreateMap<StarBoardRequestDto, StarredBoard>();
        CreateMap<StarredBoard, StarredBoardIDDto>();
        CreateMap<UnStarBoardRequestDto, StarredBoard>();
        CreateMap<StarredBoardDto, Board>();
    }
}