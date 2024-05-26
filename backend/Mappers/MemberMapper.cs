using backend.DTOs.Members.Output;
using backend.Models;
namespace backend.Mappers;

public static class MemberMapper
{
 
    
    public static MemberDto ToMemberDto(this Members memberModel)
    {
        return new MemberDto
        {
            MemberId = memberModel.MemberId,
            UserId = memberModel.UserId
        };
    }
}
