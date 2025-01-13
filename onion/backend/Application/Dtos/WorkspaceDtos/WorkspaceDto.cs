using System.Diagnostics;
using Application.Dtos.BoardDtos;
using Application.Dtos.MembersDtos;
using Application.Dtos.WorkspaceActivityDto;
using Domain.Entities;

namespace Application.Dtos.WorkspaceDtos;

public class WorkspaceDto
{
    public int WorkspaceId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string OwnerId { get; set; }
    public List<MemberDto> Members { get; set; } = new List<MemberDto>();
    public List<BoardDto> Boards { get; set; } = new List<BoardDto>();

    //Constructor
    public WorkspaceDto(Workspace workspace)
    {
        WorkspaceId = workspace.WorkspaceId;
        Title = workspace.Title;
        Description = workspace.Description;
        OwnerId = workspace.OwnerId;

        if (workspace.Members != null) 
        {
            foreach (var member in workspace.Members)
            {
                Members.Add(new MemberDto(member));
            }
        }

        if (workspace.Boards != null) 
        {
            foreach (var board in workspace.Boards)
            {
                Boards.Add(new BoardDto(board));
            }
        }
    }

}