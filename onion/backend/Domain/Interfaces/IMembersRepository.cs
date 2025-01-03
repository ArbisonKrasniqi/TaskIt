using Domain.Entities;

namespace Domain.Interfaces;

public interface IMembersRepository
{
    Task<IEnumerable<Members>> GetMembers(
        int? memberId = null,
        string userId = null,
        int? workspaceId = null,
        DateTime? dateJoined = null
    );

    Task<Members> CreateMember(Members member);
    Task<Members> UpdateMember(Members member);
    Task<Members> DeleteMember(int memberId);
}