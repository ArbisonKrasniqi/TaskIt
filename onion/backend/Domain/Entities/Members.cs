using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Members
{
    [Key] public int MemberId { get; set; }
    public string UserId { get; set; }
    public int WorkspaceId { get; set; }
    public DateTime DateJoined { get; set; }
    //Navigation
    public User User { get; set; }
    public Workspace Workspace { get; set; }

    //Constructors
    public Members() {}

    public Members(int memberId, string userId, int workspaceId, DateTime dateJoined)
    {
        MemberId = memberId;
        UserId = userId;
        WorkspaceId = workspaceId;
        DateJoined = dateJoined;
    }

    public Members(string userId, int workspaceId, DateTime dateJoined)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
        DateJoined = dateJoined;
    }
}