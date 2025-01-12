using Domain.Entities;

namespace Application.Dtos.MembersDtos;

public class MemberDto
{
    public int MemberId { get; set; }
    public string UserId { get; set; }
    public int WorkspaceId { get; set; }
    public DateTime DateJoined { get; set; }

    //Constructor
    public MemberDto(Members member)
    {
        MemberId = member.MemberId;
        UserId = member.UserId;
        WorkspaceId = member.WorkspaceId;
        DateJoined = member.DateJoined;
    }
}