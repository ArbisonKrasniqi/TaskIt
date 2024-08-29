using AutoMapper;
using backend.DTOs.Board.Input;
using backend.DTOs.Checklist.Input;
using backend.DTOs.Checklist.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
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

    public ChecklistController(IChecklistRepository checklistRepo, ITaskRepository taskRepo,
        IListRepository listRepo,IBoardRepository boardRepo, IMembersRepository membersRepo,IMapper mapper)
    {
        _checklistRepo = checklistRepo;
        _taskRepo = taskRepo;
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _membersRepo = membersRepo;
        _mapper = mapper;
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistId);

            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            var taskId = checklist.TaskId;
            var isMember = await _membersRepo.IsAMember(userId, taskId);

            if (isMember || userTokenRole == "Admin")
            {
                var checklistDTO = _mapper.Map<ChecklistDTO>(checklist);
                return Ok(checklistDTO);
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

        if (!await _taskRepo.TaskExists(checklistDto.TaskId))
        {
            return BadRequest("Task not found");
        }
        
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _membersRepo.IsAMember(userId, checklistDto.TaskId);
            if (isMember || userTokenRole == "Admin")
            {
                var checklistModel = _mapper.Map<Checklist>(checklistDto);
                await _checklistRepo.CreateChecklistAsync(checklistModel);
                return CreatedAtAction(nameof(GetChecklistById), new { id = checklistModel.ChecklistId },
                    _mapper.Map<ChecklistDTO>(checklistModel));
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!" + e.Message);
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
            if (!await _taskRepo.TaskExists(task.TaskId))
            {
                return NotFound("Task Not Found");
            }

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (!await _listRepo.ListExists(list.ListId))
            {
                return NotFound("List not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (!await _boardRepo.BoardExists(board.BoardId))
            {
                return NotFound("Board Not Found");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var workspaceId = board.WorkspaceId;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var checklistModel = await _checklistRepo.UpdateChecklistAsync(checklist);

                if (checklistModel == null)
                {
                    return NotFound("Checklist not found");
                }

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
    // delete chechklist missing

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetChecklistByTaskId")]
    public async Task<IActionResult> GetChecklistByTaskId(int taskId)
    {
        try
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskId);
            if (!await _taskRepo.TaskExists(task.TaskId))
            {
                return NotFound("Task Not Found");
            }

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (!await _listRepo.ListExists(list.ListId))
            {
                return NotFound("List not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (!await _boardRepo.BoardExists(board.BoardId))
            {
                return NotFound("Board Not Found");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var workspaceId = board.WorkspaceId;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
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
        if (!await _taskRepo.TaskExists(taskIdDto.TaskId))
        {
            return StatusCode(404, "Task Not Found");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskIdDto.TaskId);
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (!await _listRepo.ListExists(list.ListId))
            {
                return NotFound("List not found");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (!await _boardRepo.BoardExists(board.BoardId))
            {
                return NotFound("Board not found ");
            }
            var workspaceId = board.WorkspaceId;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
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