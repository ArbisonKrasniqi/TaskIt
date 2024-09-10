using System.ComponentModel.DataAnnotations;
using backend.DTOs.StarredBoard;

namespace backend.Models;

public class StarredBoard
{
    [Key]
    public int StarredBoardId { get; set; }
    public int BoardId { get; set; }
    public Board Board { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int WorkspaceId { get; set; }
    public Workspace Workspace { get; set; }
}