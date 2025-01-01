using Domain.Entities;

namespace Application.Dtos.UserDtos;

public class GetUserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsDeleted { get; set; }
    public string Role { get; set; }

    public GetUserDto(User user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        DateCreated = user.DateCreated;
        IsDeleted = user.IsDeleted;
        Role = user.Role;
    }
}