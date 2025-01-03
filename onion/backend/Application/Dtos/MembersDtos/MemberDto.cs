namespace Application.Dtos.MembersDtos;

public class MemberDto
{
    public int MemberId { get; set; }
    public string UserId { get; set; }
    public int WorkspaceId { get; set; }
    public DateTime DateJoined { get; set; }

    //Constructor
    public MemberDto(int memberId, string userId, int workspaceId, DateTime dateJoined)
    {
        MemberId = memberId;
        UserId = userId;
        WorkspaceId = workspaceId;
        DateJoined = dateJoined;
    }
}