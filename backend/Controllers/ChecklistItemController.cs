using AutoMapper;
using backend.DTOs.Checklist.Input;
using backend.DTOs.ChecklistItem.Input;
using backend.DTOs.ChecklistItem.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace backend.Controllers;
[Route("backend/checklistItems")]
[ApiController]
public class ChecklistItemController : ControllerBase
{
    private readonly IChecklistItemRepository _checklistItemRepo;
    private readonly IChecklistRepository _checklistRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly IMapper _mapper;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IUserRepository _userRepo;
    private readonly IBoardActivityRepository _boardActivityRepo;

    public ChecklistItemController(IChecklistItemRepository checklistItemRepo, IChecklistRepository checklistRepo,
        ITaskRepository taskRepo,
        IListRepository listRepo, IBoardRepository boardRepo, IMembersRepository membersRepo, IMapper mapper, IWorkspaceRepository workspaceRepo, IUserRepository userRepo, IBoardActivityRepository boardActivityRepo)
    {
        _checklistItemRepo = checklistItemRepo;
        _checklistRepo = checklistRepo;
        _taskRepo = taskRepo;
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _membersRepo = membersRepo;
        _mapper = mapper;
        _workspaceRepo = workspaceRepo;
        _userRepo = userRepo;
        _boardActivityRepo = boardActivityRepo;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet(template: "GetAllChecklistItems")]
    public async Task<IActionResult> GetAllChecklistItems()
    {
        try
        {
            var checklistItems = await _checklistItemRepo.GetAllChecklistItemsAsync();
            if (checklistItems.Count() == 0)
            {
                return NotFound("There are no checklistItems");
            }

            var checklistItemDTO = _mapper.Map<IEnumerable<ChecklistItemDTO>>(checklistItems);
            return Ok(checklistItemDTO);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!" + e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetChecklistItemById")]
    public async Task<IActionResult> GetChecklistItemById(int checklistItemId)
    {
        try
        {
            var checklistItem = await _checklistItemRepo.GetChecklistItemByIdAsync(checklistItemId);
            if (checklistItem == null)
            {
                return NotFound("ChecklistItem not found");
            }
            
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistItem.ChecklistId);
            if (checklist ==null)
            {
                return NotFound("Checklist not found");
            }
            var task = await _taskRepo.GetTaskByIdAsync(checklist.TaskId);
            if (task == null)
            {
                return NotFound("Task not found");
            }

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List not found");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board not found");
            }
            
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }

            var workspaceId = board.WorkspaceId;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            if (isMember || userTokenRole == "Admin")
            {
                var checklistItemDto = _mapper.Map<ChecklistItemDTO>(checklistItem);
                return Ok(checklistItemDto);
            }
            return StatusCode(401, "You arent authorized");

        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!" + e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateChecklistItem")]
    public async Task<IActionResult> CreateChecklistItem(CreateChecklistItemDTO checklistItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistItemDto.ChecklistId);
            if (checklist == null)
            {
                return BadRequest("Checklist not found");
            }

            var task = await _taskRepo.GetTaskByIdAsync(checklist.TaskId);
            if (task == null)
            {
                return BadRequest("Task not found");
            }
            
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return BadRequest("List not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return BadRequest("Board not found");
            }
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var isMember = await _membersRepo.IsAMember(userId, checklistItemDto.ChecklistId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            if (isMember || userTokenRole == "Admin")
            {
                var checklistItemModel = _mapper.Map<ChecklistItem>(checklistItemDto);
                await _checklistItemRepo.CreateChecklistItemAsync(checklistItemModel);

                //Created BoardActivity
                var boardActivity = new BoardActivity{
                    BoardId = checklistItemModel.ChecklistItemId,
                    UserId = userId,
                    ActionType = "created",
                    EntityName = "checklistItem " + checklistItemDto.Content,
                    ActionDate = DateTime.Now
                };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);


                return CreatedAtAction(nameof(GetChecklistItemById),new {id= checklistItemModel.ChecklistItemId},_mapper
                    .Map<ChecklistItemDTO>(checklistItemModel));
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!" + e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateChecklistItem")]
    public async Task<IActionResult> UpdateChecklistItem(UpdateChecklistItemDTO checklistItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistItemDto.ChecklistId);
            if (checklist == null)
            {
                return BadRequest("Checklist not found");
            }

            var task = await _taskRepo.GetTaskByIdAsync(checklist.TaskId);
            if (task == null)
            {
                return BadRequest("Task not found");
            }
            
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return BadRequest("List not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return BadRequest("Board not found");
            }
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            var isMember = await _membersRepo.IsAMember(userId, checklistItemDto.ChecklistId);
            if (isMember || userTokenRole == "Admin")
            {
                var checklistItemModel = await _checklistItemRepo.UpdateChecklistItemAsync(checklistItemDto);
                if (checklistItemDto == null)
                {
                    return NotFound("ChecklistItem not found");
                }

                //Updated BoardActivity
                var boardActivity = new BoardActivity{
                    BoardId = checklistItemModel.ChecklistItemId,
                    UserId = userId,
                    ActionType = "updated",
                    EntityName = "checklistItem " + checklistItemDto.Content,
                    ActionDate = DateTime.Now
                };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);



                var checklistItem = _mapper.Map<ChecklistItemDTO>(checklistItemModel);
                return Ok(checklistItem);
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteChecklistItem")]
    public async Task<IActionResult> DeleteChecklistItem(ChecklistItemIdDTO checklistItemIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var checklistItem = await _checklistItemRepo.GetChecklistItemByIdAsync(checklistItemIdDto.ChecklistItemId);
            if (checklistItem == null)
            {
                return NotFound("ChecklistItem not found");
            }

            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistItem.ChecklistId);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            var task = await _taskRepo.GetTaskByIdAsync(checklist.TaskId);
            if (task == null)
            {
                return NotFound("Task not found");
            }

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List not found");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board not found");
            }
            
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            var isMember = await _membersRepo.IsAMember(userId, workspace.WorkspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var checklistItemModel = await _checklistItemRepo.DeleteChecklistItemAsync(checklistItemIdDto.ChecklistItemId);

                //Deleted BoardActivity
                var boardActivity = new BoardActivity{
                    BoardId = checklistItemModel.ChecklistItemId,
                    UserId = userId,
                    ActionType = "deleted",
                    EntityName = "checklistItem " + checklistItemIdDto,
                    ActionDate = DateTime.Now
                };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);


                return Ok("ChecklistItem deleted");
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetChecklistItemByChecklistId")]
    public async Task<IActionResult> GetChecklistItemByChecklistId(int checklistId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistId);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            var task = await _taskRepo.GetTaskByIdAsync(checklist.TaskId);
            if (task == null)
            {
                return NotFound("Task not found");
            }
            
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board not found");
            }
            
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            var isMember = await _membersRepo.IsAMember(userId, workspace.WorkspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var checklistItems = await _checklistItemRepo.GetChecklistItemByChecklistIdAsync(checklistId);
                if (checklistItems.Count == 0)
                {
                    return Ok(new List<ChecklistItemDTO>());
                }

                var checklistItemDto = _mapper.Map<IEnumerable<ChecklistItemDTO>>(checklistItems);
                return Ok(checklistItemDto);
            }
            return StatusCode(401,"You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!"+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete(template: "DeleteChecklistItembyChecklistId")]
    public async Task<IActionResult> DeleteChecklistItembyChecklistId(ChecklistIdDTO checklistIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistIdDto.ChecklistId);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            var task = await _taskRepo.GetTaskByIdAsync(checklist.TaskId);
            if (task == null)
            {
                return NotFound("Task not found");
            }
            
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board not found");
            }
            
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            var isMember = await _membersRepo.IsAMember(userId, workspace.WorkspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var checklistItemModel =
                    await _checklistItemRepo.DeleteChecklistItemByChecklistIdAsync(checklistIdDto.ChecklistId);
                if (checklistItemModel.Count == 0)
                {
                    return NotFound("ChecklistItem not found");
                }

                return Ok("ChecklistItem Deleted");
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!");
        }
    }
    
    [HttpPut(template:"ChangeChecklistItemChecked")]
    public async Task<IActionResult> ChangeChecklistItemChecked(int checklistItemId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            if (checklistItemId <= 0)
            {
                return BadRequest("Wrong checklistItem Id");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var checklistItem = await _checklistItemRepo.ChangeChecklistItemChecked(checklistItemId);

                //Checked BoardActivity
                var boardActivity = new BoardActivity{
                    BoardId = checklistItem.ChecklistItemId,
                    UserId = userId,
                    ActionType = "checked",
                    EntityName = "checklistItem " + checklistItemId,
                    ActionDate = DateTime.Now
                };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);

            

            return Ok("ChecklistItem changed to: "+checklistItem.Checked);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }


}