using backend.DTOs.TaskActivity.Input;
using backend.DTOs.TaskActivity.Output;
using backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Mappers.TaskActivity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace backend.Controllers;

public class TaskActivityController : ControllerBase
{
    private readonly ITaskActivityRepository _taskActivityRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IUserRepository _userRepo;

    public TaskActivityController(ITaskActivityRepository taskActivityRepo, 
        ITaskRepository taskRepo, 
        IMembersRepository membersRepo, 
        IListRepository listRepo,
        IBoardRepository boardRepo, 
        IUserRepository userRepo)
    {
        _taskActivityRepo = taskActivityRepo;
        _taskRepo = taskRepo;
        _membersRepo = membersRepo;
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _userRepo = userRepo;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllTaskActivities")]
    public async Task<IActionResult> GetAllTaskActivities()
    {
        try
        {
            var taskActivities = await _taskActivityRepo.GetAllTasksActivityAsync();
            if (taskActivities.Count == 0)
            {
                return Ok(new List<TaskActivityDto>());
            }

            return Ok(taskActivities);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error! "+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetTaskActivityById")]
    public async Task<IActionResult> GetTaskActivityById(int id)
    {
        try
        {
            var taskActivity = await _taskActivityRepo.GetTaskActivityByIdAsync(id);
            if (taskActivity == null)
            {
                return NotFound("Task Activity Not Found!");
            }

            var task = await _taskRepo.GetTaskByIdAsync(taskActivity.TaskId);
            if (task == null)
            {
                return null;
            }

            return Ok(taskActivity);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error! " + e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTaskActivityByTaskId")]
    public async Task<IActionResult> GetTaskActivityByTaskId(TaskIdDTO taskIdDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var task = await _taskRepo.GetTaskByIdAsync(taskIdDto.TaskId);
            if(task == null){
                return NotFound("Task not found");
            }
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("Parent list not found");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board Not Found");
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
                var taskActivities = await _taskActivityRepo.GetTaskActivitiesByTaskId(taskIdDto.TaskId);
                if (taskActivities.Count == 0)
                {
                    return Ok(new List<TaskActivityDto>());
                }

                return Ok(taskActivities);
            }
            return StatusCode(401, "You are not authorized!");
            }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: "+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateTaskActivity")]
    public async Task<IActionResult> CreateTaskActivity(AddTaskActivityDto taskActivityDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var task = await _taskRepo.GetTaskByIdAsync(taskActivityDto.TaskId);
            if(task == null){
                return NotFound("Task not found");
            }
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("Parent list not found");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board Not Found");
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
                var taskActivity = taskActivityDto.ToTaskActivityFromCreate(userId);
                await _taskActivityRepo.CreateTaskActivityAsync(taskActivity);
                return CreatedAtAction(nameof(GetTaskActivityById),
                    new { id = taskActivity.TaskActivityId }, taskActivity);
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error! "+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteTaskActivity")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteTaskActivity(TaskActivityIdDto taskActivityIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var taskActivity = await _taskActivityRepo.GetTaskActivityByIdAsync(taskActivityIdDto.TaskActivityId);
            if (taskActivity == null)
            {
                return NotFound("Task Activity Not Found!");
            }

            var task = await _taskRepo.GetTaskByIdAsync(taskActivity.TaskId);
            if (task == null)
            {
                return NotFound("Task Not Found!");
            }

            var taskActivityModel = await _taskActivityRepo.DeleteTaskActivityByIdAsync(taskActivityIdDto.TaskActivityId);
            if (taskActivityModel == null)
            {
                return NotFound("Task Activity Not Found To Delete!");
            }

            return Ok("Task Activity Deleted!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteTaskActivityByTaskId")]
    public async Task<IActionResult> DeleteTaskActivityByTaskId(TaskIdDTO taskIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskIdDto.TaskId);
            if(task == null){
                return NotFound("Task not found");
            }
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("Parent list not found");
            }
            
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board Not Found");
            }
            
            var workspaceId = board.WorkspaceId;
            
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (userId == null)
            {
                return NotFound("User Not Found!");
            }
            
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if (isOwner || userTokenRole == "Admin")
            {
                var taskActivities = await _taskActivityRepo.DeleteTaskActivitiesByTaskId(taskIdDto.TaskId);
                if (taskActivities.Count == 0)
                {
                    return Ok(new List<TaskActivityDto>());
                }

                return Ok(taskActivities);
            }

            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }
}