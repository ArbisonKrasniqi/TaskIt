namespace backend.Models;

public class Members
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime DateJoined { get; set; }
    public User User { get; set; }
    public int WorkspaceId { get; set; }
}