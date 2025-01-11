using Application.Handlers.StarredBoard;
using Domain.Interfaces;

namespace Application.Handlers.Board;

public class BoardDeleteHandler : IBoardDeleteHandler
{
    private readonly IBoardRepository _boardRepository;
    //private readonly IListDeleteHandler _listDeleteHandler;
    private readonly IStarredBoardDeleteHandler _starredBoardDeleteHandler;

    public BoardDeleteHandler(IBoardRepository boardRepository /*, IListDeleteHandler listDeleteHandler*/, IStarredBoardDeleteHandler starredBoardDeleteHandler)
    {
        _boardRepository = boardRepository;
        //_listDeleteHandler = listDeleteHandler;
        _starredBoardDeleteHandler = starredBoardDeleteHandler;
    }


    public async Task HandleDeleteRequest(int boardId)
    {
        var boardLists = (await _boardRepository.GetBoards(boardId: boardId)).FirstOrDefault().Lists;
        if (boardLists.Any())
        {
            //Nese ka lista ne board, krijo nje loop qe iteron ne te gjitha listat
            //dhe i fshin ato lista permes ListDeleteHandler
            /*
             * foreach (list in boardLists) {
             *      await _listDeleteHandler.HandleDeleteRequest(list.ListId);
             * }
             */
        }
        
        var starredBoards = (await _boardRepository.GetBoards(boardId: boardId)).FirstOrDefault().StarredBoards;
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