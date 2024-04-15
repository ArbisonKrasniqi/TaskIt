using backend.DTOs.User;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Mappers;

public class UserMappers
{
    
    public static GetUserDTO ToGetUserDTO(User user)
    {
        return new GetUserDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        };
    }
    
}