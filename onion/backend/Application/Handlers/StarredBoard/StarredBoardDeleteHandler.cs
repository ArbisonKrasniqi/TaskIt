using Domain.Interfaces;

namespace Application.Handlers.StarredBoard;

public class StarredBoardDeleteHandler : IStarredBoardDeleteHandler
{
    private readonly IStarredBoardRepository _starredBoardRepository;

    public StarredBoardDeleteHandler(IStarredBoardRepository starredBoardRepository)
    {
        _starredBoardRepository = starredBoardRepository;
    }


    public async Task HandleDeleteRequest(int starredBoardId)
    {
        await _starredBoardRepository.DeleteStarredBoard(starredBoardId);
    }
}