using Domain.Entities;

namespace Application.Dtos.UserDtos;

public class UserInfoDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    //Qiky construtor po paraqet problem ne UserController.cs
    public UserInfoDto(User user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
    }
}

