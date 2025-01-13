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

    public BoardDeleteHandler(IBoardRepository boardRepository, IListDeleteHandler listDeleteHandler, IStarredBoardDeleteHandler starredBoardDeleteHandler, IWorkspaceActivityRepository workspaceActivityRepository)
    {
        _boardRepository = boardRepository;
        _listDeleteHandler = listDeleteHandler;
        _starredBoardDeleteHandler = starredBoardDeleteHandler;
        _workspaceActivityRepository = workspaceActivityRepository;
    }


    public async Task HandleDeleteRequest(int boardId)
    {
        var boardLists = (await _boardRepository.GetBoards(boardId: boardId)).FirstOrDefault().Lists.ToList();
        if (boardLists.Any())
        {
              foreach (var list in boardLists) {
                   await _listDeleteHandler.HandleDeleteRequest(list.ListId);
              }
             
        }
        
        var starredBoards = (await _boardRepository.GetBoards(boardId: boardId)).FirstOrDefault().StarredBoards.ToList();
        if (starredBoards.Any())
        {
            foreach (var board in starredBoards)
            {
                await _starredBoardDeleteHandler.HandleDeleteRequest(board.StarredBoardId);
            }
        }

        await _boardRepository.DeleteBoard(boardId);
    }
}