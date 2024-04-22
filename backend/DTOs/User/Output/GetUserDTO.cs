namespace backend.DTOs.User.Output;

public class GetUserDTO
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime DateCreated { get; set; }
}