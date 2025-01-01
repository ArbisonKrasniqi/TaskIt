using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    //Properties
    [Key]
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsDeleted { get; set; }
    public string Role { get; set; }
    public string RefreshToken { get; set; }
    
    //Navigation
    // public List<Workspace> Workspaces { get; set; } = new List<Workspace>();
    // public List<Comment> Comments { get; set; } = new List<Comment>();
    // public List<WorkspaceActivity> Activity { get; set; } = new List<WorkspaceActivity>();
    // public List<Invite> SentInvites { get; set; } = new List<Invite>();
    // public List<Invite> ReceivedInvites { get; set; } = new List<Invite>();
    // public List<StarredBoard> StarredBoards { get; set; } = new List<StarredBoard>();
    
    //Constructors
    public User(){}
    
    public User(string firstName, string lastName, DateTime dateCreated, bool isDeleted)
    {
        FirstName = firstName;
        LastName = lastName;
        DateCreated = dateCreated;
        IsDeleted = isDeleted;
    }

    public User(string id, string firstName, string lastName,string email, string passwordHash, DateTime dateCreated, bool isDeleted, string role, string refreshToken)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        DateCreated = dateCreated;
        IsDeleted = isDeleted;
        Role = role;
        RefreshToken = refreshToken;
    }
}