using Application.Dtos.TasksDtos;
using Application.Services.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;


[ApiController]
[Route("backend/user")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    

    public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
    {
        _taskService = taskService;
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllTasks")]
    public async Task<IActionResult> GetAllTasks(){
        try
        {
            var tasks = await _taskService.GetAllTasks();
            return Ok(tasks);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTaskById")]
    public async Task<IActionResult> GetTaskById(int taskId)
    {
        try
        {
            //Check if user has access to task
            //AuthenticationService
            
            if (Int32.IsNegative(taskId)) return BadRequest("Task Id is invalid");

            var task = await _taskService.GetTaskById(taskId);

            return Ok(task);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTasksByWorkspaceId")]
    public async Task<IActionResult> GetTasksByWorkspaceId(int workspaceId)
    {
        try
        {
            //Check if user has access to workspace
            //AuthenticationService
        
            if (Int32.IsNegative(workspaceId)) return BadRequest("Workspace Id is invalid");

            var tasks = await _taskService.GetTasksByWorkspaceId(workspaceId);

            return Ok(tasks);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTasksByBoardId")]
    public async Task<IActionResult> GetTasksByBoardId(int boardId)
    {
        try
        {
            //Check if user has access to board
            //AuthenticationService
        
            if (Int32.IsNegative(boardId)) return BadRequest("Board Id is invalid");

            var tasks = await _taskService.GetTasksByBoardId(boardId);

            return Ok(tasks);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTaskByListId")]
    public async Task<IActionResult> GetTasksByListId(int listId)
    {
        try
        {
            //Check if user has access to list
            //AuthenticationService
            
            if (Int32.IsNegative(listId)) return BadRequest("List Id is invalid");

            var tasks = await _taskService.GetTasksByListId(listId);

            return Ok(tasks);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateTask")]
    public async Task<IActionResult> CreateTask(CreateTaskDto createTaskDto)
    {
        try
        {
            //Check if user has access to list
            //AuthenticationService
            
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var task = await _taskService.CreateTask(createTaskDto);

            return Ok(task);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteTask")]
    public async Task<IActionResult> DeleteTask(TaskIdDto taskIdDto)
    {
        try
        {
            //Check if user has access to task
            //AuthenticationService
            
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var task = await _taskService.DeleteTask(taskIdDto);

            return Ok(task);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateTask")]
    public async Task<IActionResult> UpdateTask(UpdateTaskDto updateTaskDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var task = await _taskService.UpdateTask(updateTaskDto); 
            
            return Ok(task);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}