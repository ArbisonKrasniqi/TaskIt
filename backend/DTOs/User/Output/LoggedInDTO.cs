namespace backend.DTOs.User.Output;

public class LoggedInDTO
//This DTO is used whenever a user logs into an account, this is what they get in return.
{
    public string accessToken { get; set; }
    public string refreshToken { get; set; }
}