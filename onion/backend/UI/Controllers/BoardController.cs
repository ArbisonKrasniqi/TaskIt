using Application.Dtos.BoardDtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/board")]
public class BoardController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllBoards")]
    public async Task<IActionResult> GetAllBoards()
    {
        try
        {
            var boards = await _boardService.GetAllBoards();
            return Ok(boards);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetBoardsByWorkspaceId")]
    public async Task<IActionResult> GetBoardsByWorkspaceId(int workspaceId)
    {
        try
        {
            if (Int32.IsNegative(workspaceId))
                return BadRequest("Workspace Id is invalid");

            var boards = await _boardService.GetBoardsByWorkspaceId(workspaceId);

            return Ok(boards);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetBoardById")]
    public async Task<IActionResult> GetBoardById(int boardId)
    {
        try
        {
            if (Int32.IsNegative(boardId))
                return BadRequest("Board Id is invalid");

            var board = await _boardService.GetBoardById(boardId);

            return Ok(board);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetClosedBoards")]
    public async Task<IActionResult> GetClosedBoards(int workspaceId)
    {
        try
        {
            if (Int32.IsNegative(workspaceId))
                return BadRequest("Workspace Id is invalid");

            var closedBoards = await _boardService.GetClosedBoards(workspaceId);

            return Ok(closedBoards);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateBoard")]
    public async Task<IActionResult> CreateBoard(CreateBoardDto createBoardDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var board = await _boardService.CreateBoard(createBoardDto);

            return Ok(board);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateBoard")]
    public async Task<IActionResult> UpdateBoard(UpdateBoardDto updateBoardDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedBoard = await _boardService.UpdateBoard(updateBoardDto);

            return Ok(updatedBoard);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("CloseBoard")]
    public async Task<IActionResult> CloseBoard(BoardIdDto boardIdDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var closedBoard = await _boardService.CloseBoard(boardIdDto);

            return Ok(closedBoard);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("OpenBoard")]
    public async Task<IActionResult> OpenBoard(BoardIdDto boardIdDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var openBoard = await _boardService.OpenBoard(boardIdDto);

            return Ok(openBoard);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteBoard")]
    public async Task<IActionResult> DeleteBoard(BoardIdDto boardIdDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deletedBoard = await _boardService.DeleteBoard(boardIdDto);

            return Ok(deletedBoard);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}