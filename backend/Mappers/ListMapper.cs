using backend.DTOs.List;
using backend.Models;

namespace backend.Mappers;

public static class ListMapper
{
    public static ListDTO ToListDto(this List listModel)
    {
        return new ListDTO
        {
            ListId = listModel.ListId,
            Title = listModel.Title, 
            BoardId = listModel.BoardId,
            DateCreated = listModel.DateCreated
        };
    }
    
    public static List ToListFromCreate(this CreateListDTO listDto)
    {
        return new List
        {
            Title = listDto.Title,
            DateCreated = listDto.DateCreated,
            BoardId = listDto.BoardId
        };
    }

    public static List ToListFromUpdate(this UpdateListDTO listDto)
    {
        return new List
        {
            Title = listDto.Title
        };
    }
}