using Application.Handlers.List;
using Application.Handlers.StarredBoard;
using Domain.Interfaces;

namespace Application.Handlers.Board;

public class BoardDeleteHandler : IBoardDeleteHandler
{
    private readonly IBoardRepository _boardRepository;
    private readonly IListDeleteHandler _listDeleteHandler;
    private readonly IStarredBoardDeleteHandler _starredBoardDeleteHandler;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepository;
    private readonly UserContext _userContext;

    public BoardDeleteHandler(IBoardRepository boardRepository, IListDeleteHandler listDeleteHandler, IStarredBoardDeleteHandler starredBoardDeleteHandler, IWorkspaceActivityRepository workspaceActivityRepository, UserContext userContext)
    {
        _boardRepository = boardRepository;
        _listDeleteHandler = listDeleteHandler;
        _starredBoardDeleteHandler = starredBoardDeleteHandler;
        _workspaceActivityRepository = workspaceActivityRepository;
        _userContext = userContext;
    }


    public async Task HandleDeleteRequest(int boardId)
    {
        var board = (await _boardRepository.GetBoards(boardId: boardId)).FirstOrDefault();
        var boardLists = board.Lists.ToList();
        if (boardLists.Any())
        {
              foreach (var list in boardLists) {
                   await _listDeleteHandler.HandleDeleteRequest(list.ListId);
              }
             
        }
        
        var starredBoards = (await _boardRepository.GetBoards(boardId: boardId)).FirstOrDefault().StarredBoards.ToList();
        if (starredBoards.Any())
        {
            foreach (var starredBoard in starredBoards)
            {
                await _starredBoardDeleteHandler.HandleDeleteRequest(starredBoard.StarredBoardId);
            }
        }

        await _boardRepository.DeleteBoard(boardId);
        
        var newActivity = new Domain.Entities.WorkspaceActivity(board.WorkspaceId,
            _userContext.Id,
            "Deleted",
            board.Title,
            DateTime.Now);
        await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);
    }
}