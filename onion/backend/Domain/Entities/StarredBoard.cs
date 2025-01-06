using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class StarredBoard
{
    [Key]
    public int StarredBoardId { get; set; }
    public int BoardId { get; set; }
    public string UserId { get; set; }
    public int WorkspaceId { get; set; }
    
    //Navigation
    //public Board Board { get; set; }
    //public Uset User { get; set; }
    //public Workspace Workspace { get; set; }
    
    public StarredBoard(){}

    public StarredBoard(int boardId, string userId, int workspaceId)
    {
        BoardId = boardId;
        UserId = userId;
        WorkspaceId = workspaceId;
    }

    public StarredBoard(int starredBoardId, int boardId, string userId, int workspaceId)
    {
        StarredBoardId = starredBoardId;
        BoardId = boardId;
        UserId = userId;
        WorkspaceId = workspaceId;
    }
}