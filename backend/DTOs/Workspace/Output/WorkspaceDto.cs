using backend.DTOs.Board.Output;
using backend.DTOs.Members.Output;
    
namespace backend.DTOs.Workspace;

public class WorkspaceDto
{
    public int WorkspaceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OwnerId { get; set; }
    public List<MemberDto> Members { get; set; }= new List<MemberDto>();
    public List<BoardDto> Boards { get; set; }=new List<BoardDto>();

}