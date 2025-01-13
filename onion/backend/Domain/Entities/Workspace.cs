using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class Workspace
{
    
    [Key] public int WorkspaceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public string OwnerId { get; set; }
    
    //Navigation
    public User User { get; set; }
    
    public List<Board> Boards { get; set; }
    public List<Members> Members { get; set; }
    public List<Invite> Invites { get; set; }
    public List<WorkspaceActivity> Activity { get; set; }
    public List<StarredBoard> StarredBoards {get; set;}

    //Constructors
    public Workspace(){}
    
    public Workspace(int workspaceId, string title, string description, DateTime dateCreated, string ownerId)
    {
        WorkspaceId = workspaceId;
        Title = title;
        Description = description;
        DateCreated = dateCreated;
        OwnerId = ownerId;
     
    }
 
    public Workspace(string title, string description, DateTime dateCreated, string ownerId)
    {
        Title = title;
        Description = description;
        DateCreated = dateCreated;
        OwnerId = ownerId;
    }
    

}