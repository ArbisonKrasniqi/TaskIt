using AutoMapper;
using backend.DTOs.Board.Input;
using backend.DTOs.List;
using backend.Interfaces;
using backend.Mappers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace backend.Controllers;

[Route("backend/list")]
[ApiController]

public class ListController : ControllerBase
{
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly IUserRepository _userRepo;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMapper _mapper;
    public ListController(IListRepository listRepo , IBoardRepository boardRepo, IMembersRepository membersRepo, IUserRepository userRepo, IWorkspaceRepository workspaceRepo,IMapper mapper)
    {
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _membersRepo = membersRepo;
        _userRepo = userRepo;
        _workspaceRepo = workspaceRepo;
        _mapper = mapper;
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
            var listDto = _mapper.Map<IEnumerable<ListDTO>>(lists);
            
            return Ok(listDto);
        }
        catch (Exception e)
        {
            return StatusCode(500,"Internal Server Error!"+e.Message);
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetListById")]
    public async Task<IActionResult> GetById(int listId)
    {
        try{
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

        var list = await _listRepo.GetListByIdAsync(listId);
        if (list == null)
        {
            return NotFound("List Not Found!");
        }


        var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
        if (board == null)
        {
            return NotFound("Board Not Found!");
        }


        var workspaceId = board.WorkspaceId;

        var isMember = await _membersRepo.IsAMember(userId, workspaceId);
        var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
        if (board.IsClosed && !isOwner && userTokenRole != "Admin")
        {
            return StatusCode(403, "The board is closed");
        }

        if (isMember || userTokenRole == "Admin")
        {
            var listDto = _mapper.Map<ListDTO>(list);
            return Ok(listDto);
        }
        return StatusCode(401, "You are not authorized!");
      
        }catch (Exception e)
        {
            return StatusCode(500, "Internal server error"+e.Message);
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
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                var list = await _listRepo.GetListByIdAsync(updateListDto.ListId);
                if (list == null)
                {
                    return NotFound("List not found");
                }
                
                var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
                if (board == null)
                {
                    return NotFound("Board Not Found!");
                }

                var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
                if (workspace == null)
                {
                    return NotFound("Workspace not found");
                }
                
                var workspaceId = workspace.WorkspaceId;
                var isMember = await _membersRepo.IsAMember(userId, workspaceId);
                var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
                if (board.IsClosed && !isOwner && userTokenRole != "Admin")
                {
                    return StatusCode(403, "The board is closed");
                }
                
                if (isMember || userTokenRole == "Admin")
                {
                    var listModel = await _listRepo.UpdateListAsync(updateListDto);

                    if (listModel == null)
                    {
                        return NotFound("List Not Found");
                    }

                    var listDto = _mapper.Map<ListDTO>(listModel);
                    return Ok(listDto);
                }

                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
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
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board Not Found!");
            }
            var workspaceId = board.WorkspaceId;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User Not Found!");
            }
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            if (isMember || userTokenRole == "Admin")
            {
                var listModel = await _listRepo.DeleteListAsync(listIdDto.ListId);

                if (listModel == null)
                {
                    return NotFound("List Not Found!");
                }

                return Ok("List Deleted");
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
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
            if (board == null)
            {
                return NotFound("Board Not Found!");
            }
            var workspaceId = board.WorkspaceId;
            
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }

            if (isMember || userTokenRole == "Admin")
            {
                var listModel = _mapper.Map<List>(listDto);
                await _listRepo.CreateAsync(listModel);
                return CreatedAtAction(nameof(GetById), new {id = listModel.ListId },
                    _mapper.Map<ListDTO>(listModel));
            } 
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetListByBoardId")]
    public async Task<IActionResult> GetListByBoardId(int boardId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var board = await _boardRepo.GetBoardByIdAsync(boardId);
            if (board == null)
            {
                return NotFound("Board Not Found!");
            }

            var workspaceId = board.WorkspaceId;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }

            if (isMember || userTokenRole == "Admin")
            {
                var lists = await _listRepo.GetListByBoardId(boardId);

                if (lists.Count == 0)
                {
                    return Ok(new List<ListDTO>());
                }
                var listDto = _mapper.Map<IEnumerable<ListDTO>>(lists);
                return Ok(listDto);
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
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
            if (board == null)
            {
                return NotFound("Board Not Found!");
            }
            var workspaceId = board.WorkspaceId;
            
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }

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
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }
    
}
