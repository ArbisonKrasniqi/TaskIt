using backend.DTOs.BoardActivity.Input;
using backend.DTOs.BoardActivity.Output;

namespace backend.Mappers.BoardActivity;

public static class BoardActivityMapper{
    
    public static Models.BoardActivity ToBoardActivityFromCreate(this AddBoardActivityDto boardActivityDto, string userId){
        return new Models.BoardActivity{
            BoardId = boardActivityDto.BoardId,
            UserId = userId,
            ActionType = boardActivityDto.ActionType,
            EntityName = boardActivityDto.EntityName
        };
    }

    public static BoardActivityDto ToBoardActivityDto(this Models.BoardActivity boardActivity){
        return new BoardActivityDto{
            BoardActivityId = boardActivity.BoardActivityId,
            BoardId = boardActivity.BoardId,
            UserId = boardActivity.UserId,
            ActionType = boardActivity.ActionType,
            EntityName = boardActivity.EntityName,
            ActionDate = boardActivity.ActionDate
        };
    }
}