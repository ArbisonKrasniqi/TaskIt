using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Board
{
    [Key]
    public int BoardId { get; set; }
    public string Title { get; set; }
    public bool IsClosed { get; set; }
    public int WorkspaceId { get; set; }
    public DateTime DateCreated { get; set; }
    
    //Navigation
    //public Workspace Workspace { get; set; }
    //public List<List> Lists { get; set; }
    //public List<StarredBoard> StarredBoards { get; set; }
    
    public Board(){}

    public Board(string title, int workspaceId, DateTime dateCreated)
    {
        Title = title;
        WorkspaceId = workspaceId;
        DateCreated = dateCreated;
    }

    public Board(int boardId, string title, bool isClosed, int workspaceId, DateTime dateCreated)
    {
        BoardId = boardId;
        Title = title;
        IsClosed = isClosed;
        WorkspaceId = workspaceId;
        DateCreated = dateCreated;
    }
}