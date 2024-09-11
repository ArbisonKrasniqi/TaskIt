using AutoMapper;
using backend.DTOs.Board.Input;
using backend.DTOs.Checklist.Input;
using backend.DTOs.Checklist.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace backend.Controllers;

[Route("backend/checklist")]
[ApiController]
public class ChecklistController : ControllerBase
{
    private readonly IChecklistRepository _checklistRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IBoardActivityRepository _boardActivityRepo;
    private readonly ITaskActivityRepository _taskActivityRepo;

    public ChecklistController(IChecklistRepository checklistRepo, ITaskRepository taskRepo,
        IListRepository listRepo,IBoardRepository boardRepo, IMembersRepository membersRepo,IMapper mapper, IUserRepository userRepo, IWorkspaceRepository workspaceRepo, ITaskActivityRepository taskActivityRepo, IBoardActivityRepository boardActivityRepo)
    {
        _checklistRepo = checklistRepo;
        _taskRepo = taskRepo;
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _membersRepo = membersRepo;
        _mapper = mapper;
        _userRepo = userRepo;
        _workspaceRepo = workspaceRepo;
        _taskActivityRepo = taskActivityRepo;
        _boardActivityRepo = boardActivityRepo;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet(template: "GetAllChecklists")]
    public async Task<IActionResult> GetAllChecklists()
    {
        try
        {
            var checklists = await _checklistRepo.GetAllChecklistsAsync();

            if (checklists.Count() == 0)
            {
                return NotFound("There are no checklists");
            }

            var checklistDTO = _mapper.Map<IEnumerable<ChecklistDTO>>(checklists);

            return Ok(checklistDTO);
        }
        catch (Exception e)
        {
            return StatusCode(500,"Internal Server Error!"+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetChecklistById")]
    public async Task<IActionResult> GetChecklistById(int checklistId)
    {
        try
        {
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
            var workspaceId = board.WorkspaceId;
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            if (isMember || userTokenRole == "Admin")
            {
                var checklistDto = _mapper.Map<ChecklistDTO>(checklist);
                return Ok(checklistDto);
            }
            return StatusCode(401, "You arent authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!" + e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateChecklist")]
    public async Task<IActionResult> CreateChecklist(CreateChecklistDTO checklistDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var task = await _taskRepo.GetTaskByIdAsync(checklistDto.TaskId);
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
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var isMember = await _membersRepo.IsAMember(userId, workspace.WorkspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            if (isMember || userTokenRole == "Admin")
            {
                
                var checklistModel = _mapper.Map<Checklist>(checklistDto);
                await _checklistRepo.CreateChecklistAsync(checklistModel);
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                var taskActivity = new TaskActivity
                {
                    TaskId = task.TaskId,
                    UserId = userId,
                    ActionType = "Created",
                    EntityName = "checklist "+checklistDto.Title+" in task " + task.Title + " in list " + list.Title + " in board " + board.Title,
                    ActionDate = DateTime.Now
                };

                await _taskActivityRepo.CreateTaskActivityAsync(taskActivity);
                
                
                //Created BoardActivity
                var boardActivity = new BoardActivity{
                    BoardId = board.BoardId,
                    UserId = userId,
                    ActionType = "created",
                    EntityName = "checklist " + checklistDto.Title,
                    ActionDate = DateTime.Now
                };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);


                return CreatedAtAction(nameof(GetChecklistById), new { id = checklistModel.ChecklistId },
                    _mapper.Map<ChecklistDTO>(checklistModel));
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!" + e);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateChecklist")]
    public async Task<IActionResult> UpdateChecklist(UpdateChecklistDTO checklist)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var task = await _taskRepo.GetTaskByIdAsync(checklist.TaskId);
            if (task == null)
            {
                return NotFound("Task Not Found");
            }

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board Not Found");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            var workspaceId = board.WorkspaceId;
            
            
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            if (isMember || userTokenRole == "Admin")
            {
                var checklistModel = await _checklistRepo.UpdateChecklistAsync(checklist);

                if (checklistModel == null)
                {
                    return NotFound("Checklist not found");
                }
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                var taskActivity = new TaskActivity
                {
                    TaskId = task.TaskId,
                    UserId = userId,
                    ActionType = "Updated",
                    EntityName = "checklist "+checklist.Title+" in task " + task.Title + " in list " + list.Title + " in board " + board.Title,
                    ActionDate = DateTime.Now
                };

                await _taskActivityRepo.CreateTaskActivityAsync(taskActivity);

                //Updated BoardActivity
                var boardActivity = new BoardActivity{
                    BoardId = checklistModel.ChecklistId,
                    UserId = userId,
                    ActionType = "updated",
                    EntityName = "checklist " + checklist.Title,
                    ActionDate = DateTime.Now
                };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);

                var checklistDto = _mapper.Map<ChecklistDTO>(checklistModel);
                return Ok(checklistDto);

            }
            return StatusCode(401,"You are not authorized");
            
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteChecklist")]
    public async Task<IActionResult> DeleteChecklist(ChecklistIdDTO checklistIdDto)
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
            var isMember = await _membersRepo.IsAMember(userId, workspace.WorkspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            if (isMember || userTokenRole == "Admin")
            {
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                var taskActivity = new TaskActivity
                {
                    TaskId = task.TaskId,
                    UserId = userId,
                    ActionType = "Deleted",
                    EntityName = "checklist "+checklist.Title+" in task " + task.Title + " in list " + list.Title + " in board " + board.Title,
                    ActionDate = DateTime.Now
                };

                await _taskActivityRepo.CreateTaskActivityAsync(taskActivity);
                
                var checklistModel = await _checklistRepo.DeleteChecklistAsync(checklistIdDto.ChecklistId);

                //Deleted BoardActivity
                var boardActivity = new BoardActivity{
                    BoardId = board.BoardId,
                    UserId = userId,
                    ActionType = "deleted",
                    EntityName = "checklist " + checklist.Title,
                    ActionDate = DateTime.Now
                };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);


                
                return Ok(checklistModel);
            }
            return StatusCode(401, "You are not authorized!");

        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.StackTrace);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetChecklistByTaskId")]
    public async Task<IActionResult> GetChecklistByTaskId(int taskId)
    {
        try
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound("Task Not Found");
            }

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board Not Found");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var workspaceId = board.WorkspaceId;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            if (isMember || userTokenRole == "Admin")
            {
                var checklists = await _checklistRepo.GetChecklistByTaskIdAsync(taskId);

                if (checklists.Count == 0)
                {
                    return Ok(new List<ChecklistDTO>());
                }

                var checklistDto = _mapper.Map<IEnumerable<ChecklistDTO>>(checklists);
                return Ok(checklistDto);

            }
            return StatusCode(401,"You are not authorized");
            
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!"+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete(template: "DeleteChecklistByTaskId")]
    public async Task<IActionResult> DeleteChecklistByTaskId(TaskIdDTO taskIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskIdDto.TaskId);
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
                return NotFound("Board not found ");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var workspaceId = workspace.WorkspaceId;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            if (isMember || userTokenRole == "Admin")
            {
                var checklistModel = await _checklistRepo.DeleteChecklistByTaskIdAsync(taskIdDto.TaskId);
                if (checklistModel.Count == 0)
                {
                    return NotFound("Checklist not found");
                }

                return Ok("Checklist deleted");
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!");
        }
    }
    
    
}