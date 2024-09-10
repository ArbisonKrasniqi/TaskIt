using backend.DTOs.User;
using backend.DTOs.User.Output;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Mappers;

public static class UserMappers
{
    
    public static GetUserDTO ToGetUserDTO(Models.User user, string role)
    {
        return new GetUserDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DateCreated = user.DateCreated,
            Role = role,
            isDeleted = user.isDeleted
        };
    }
    
}