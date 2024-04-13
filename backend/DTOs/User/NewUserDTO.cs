namespace backend.DTOs.User;

public class NewUserDTO
//This DTO is used whenever a user logs into an account, this is what they get in return.
{
    public string Email { get; set; }
    public string Token { get; set; }
}