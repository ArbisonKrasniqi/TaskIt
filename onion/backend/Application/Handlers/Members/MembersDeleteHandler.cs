using Domain.Interfaces;

namespace Application.Handlers.Members;

public class MembersDeleteHandler : IMembersDeleteHandler
{
    private readonly IMembersRepository _membersRepository;

    public MembersDeleteHandler(IMembersRepository membersRepository)
    {
        _membersRepository = membersRepository;
    }

    public async Task HandleDeleteRequest(int memberId)
    {
        await _membersRepository.DeleteMember(memberId);
    }
}