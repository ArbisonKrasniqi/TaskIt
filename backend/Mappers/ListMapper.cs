﻿using backend.DTOs.List;
using backend.Models;

namespace backend.Mappers;

public static class ListMapper
{
    public static ListDTO ToListDto(this List listModel)
    {
        return new ListDTO
        {
            ListId = listModel.ListId,
            Title = listModel.Title
        };
    }
    
    public static List ToListFromCreate(this CreateListDTO listDto, int BoardId)
    {
        return new List
        {
            Title = listDto.Title,
            DateCreated = listDto.DateCreated
        };
    }

}