using backend.Data;
using backend.DTOs.Board.Input;
using backend.DTOs.List;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace backend.Controllers;

[Route("backend/list")]
[ApiController]

public class ListController : ControllerBase
{
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IMembersRepository _membersRepo;

    public ListController(IListRepository listRepo , IBoardRepository boardRepo, IMembersRepository membersRepo)
    {
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _membersRepo = membersRepo;
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllLists")]
    public async Task<IActionResult> GetAllLists()
    {
        try
        {
            var lists = await _listRepo.GetAllListsAsync();

            if (lists.Count == 0)
            {
                return NotFound("There are no lists");
            }
            var listDto = lists.Select(l => l.ToListDto());
            
            return Ok(listDto);
        }
        catch (Exception e)
        {
            return StatusCode(500,"Internal Server Error!");
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetListById")]
    public async Task<IActionResult> GetById(int ListId)
    {
        try{
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

        var list = await _listRepo.GetListByIdAsync(ListId);
        if (list == null)
        {
            return NotFound("List Not Found");
        }

        if (!await _boardRepo.BoardExists(list.BoardId))
        {
            return StatusCode(404, "Board Not Found");
        }
        var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
        var workspaceId = board.WorkspaceId;

        var isMember = await _membersRepo.IsAMember(userId, workspaceId);

        if (isMember || userTokenRole == "Admin") 
        {
            return Ok(list.ToListDto());
        }
        return StatusCode(401, "You are not authorized!");
      
        }catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateList")]

    public async Task<IActionResult> UpdateList(UpdateListDTO updateListDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (!await _boardRepo.BoardExists(updateListDto.BoardId))
        {
            return StatusCode(404, "Board Not Found");
        }
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                var board = await _boardRepo.GetBoardByIdAsync(updateListDto.BoardId);
                var workspaceId = board.WorkspaceId;
                var isMember = await _membersRepo.IsAMember(userId, workspaceId);

                if (isMember || userTokenRole == "Admin")
                {
                    var listModel = await _listRepo.UpdateListAsync(updateListDto);

                    if (listModel == null)
                    {
                        return NotFound("List Not Found");
                    }

                    return Ok(listModel.ToListDto());
                }

                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Errror!");
            }
        }
  
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteList")]

    public async Task<IActionResult> DeleteList(ListIdDTO listIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
       
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var list = await _listRepo.GetListByIdAsync(listIdDto.ListId);
            if (list == null)
            {
                return NotFound("List Not Found");
            }

            if (!await _boardRepo.BoardExists(list.BoardId))
            {
                return StatusCode(404, "Board Not Found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            var workspaceId = board.WorkspaceId;

            var isMember = await _membersRepo.IsAMember(userId, workspaceId);

            if (isMember || userTokenRole == "Admin")
            {
                var listModel = await _listRepo.DeleteListAsync(listIdDto.ListId);

                return Ok("List Deleted");
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }
    
    // Http Post with relationship one to many with board 
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateList")]

    public async Task<IActionResult> CreateList(CreateListDTO listDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            if (!await _boardRepo.BoardExists(listDto.BoardId))
            {
                return BadRequest("Board not found!");
            }
            var board = await _boardRepo.GetBoardByIdAsync(listDto.BoardId);
            var workspaceId = board.WorkspaceId;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);

            if (isMember || userTokenRole == "Admin")
            {
                var listModel = listDto.ToListFromCreate();
                await _listRepo.CreateAsync(listModel);
                return CreatedAtAction(nameof(GetById), new { ListId = listModel.ListId }, listModel.ToListDto());
            } 
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetListByBoardId")]
    public async Task<IActionResult> GetListByBoardId(int BoardId)
    {
        try
        {
            if (!await _boardRepo.BoardExists(BoardId))
            {
                return NotFound("Board Not Found");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var board = await _boardRepo.GetBoardByIdAsync(BoardId);
            var workspaceId = board.WorkspaceId;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);

            if (isMember || userTokenRole == "Admin")
            {
                var lists = await _listRepo.GetListByBoardId(BoardId);

                if (lists.Count == 0)
                {
                    return BadRequest("Lists Not Found");
                }
                var listDto = lists.Select(l => l.ToListDto());
                return Ok(listDto);
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteListByBoardId")]
    public async Task<IActionResult> DeleteListByBoardId(BoardIdDto boardIdDto)
    {
        if (!await _boardRepo.BoardExists(boardIdDto.BoardId))
        {
            return StatusCode(404, "Board Not Found");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var board = await _boardRepo.GetBoardByIdAsync(boardIdDto.BoardId);
            var workspaceId = board.WorkspaceId;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);

            if (isMember || userTokenRole == "Admin")
            {
                var listModel = await _listRepo.DeleteListsByBoardIdAsync(boardIdDto.BoardId);

                if (listModel.Count == 0)
                {
                    return NotFound("Lists not found!");
                }

                return Ok("Lists Deleted");
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!");
        }
    }
    
}
