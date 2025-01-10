using Application.Dtos.StarredBoardDtos;
using Application.Services.StarredBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/starredBoard")]
public class StarredBoardController : ControllerBase
{
    private readonly IStarredBoardService _starredBoardService;

    public StarredBoardController(IStarredBoardService starredBoardService)
    {
        _starredBoardService = starredBoardService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetStarredBoardsByUserId")]
    public async Task<IActionResult> GetStarredBoardsByUserId(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User Id is empty");

            var starredBoards = await _starredBoardService.GetStarredBoardsByUserId(userId);

            return Ok(starredBoards);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetStarredBoardsByWorkspaceId")]
    public async Task<IActionResult> GetStarredBoardsByWorkspaceId(int workspaceId)
    {
        try
        {
            if (Int32.IsNegative(workspaceId))
                return BadRequest("Workspace Id is invalid");

            var starredBoards = await _starredBoardService.GetStarredBoardsByWorkspaceId(workspaceId);

            return Ok(starredBoards);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("StarBoard")]
    public async Task<IActionResult> StarBoard(CreateStarredBoardDto createStarredBoardDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var starredBoard = await _starredBoardService.StarBoard(createStarredBoardDto);

            return Ok(starredBoard);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("UnstarBoard")]
    public async Task<IActionResult> UnstarBoard(StarredBoardIdDto starredBoardIdDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unstarredBoard = await _starredBoardService.UnstarBoard(starredBoardIdDto);

            return Ok(unstarredBoard);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}