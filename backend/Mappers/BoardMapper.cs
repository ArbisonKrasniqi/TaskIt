using backend.DTOs;
using backend.DTOs.Board.Input;
using backend.DTOs.Board.Output;
using backend.Models;

namespace backend.Mappers
{
    public static class BoardMapper
    {
        public static BoardDto ToBoardDto(this Board boardModel)
        {
            return new BoardDto
            {
                BoardId = boardModel.BoardId,
                Title = boardModel.Title,
                DateCreated = boardModel.DateCreated,
                BackgroundId = boardModel.BackgroundId,
                WorkspaceId = boardModel.WorkspaceId
            };
        }

        public static Board ToBoardFromCreate(this CreateBoardDto boardDto)
        {
            return new Board
            {
                Title = boardDto.Title,
                BackgroundId = boardDto.BackgroundId,
                WorkspaceId =   boardDto.WorkspaceId
            };
        }

        public static Board ToBoardFromUpdate(this UpdateBoardRequestDto boardDto)
        {
            return new Board
            {
                Title = boardDto.Title,
                BackgroundId = boardDto.BackgroundId,
                WorkspaceId = boardDto.WorkspaceId
            };
        }
    }
}