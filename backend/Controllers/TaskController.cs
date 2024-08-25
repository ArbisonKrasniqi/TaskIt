using backend.DTOs.Task;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs.List;
using Microsoft.AspNetCore.Authorization;
namespace backend.Controllers;


[Route("backend/task")]
[ApiController]

public class  TaskController : ControllerBase{

    private readonly IListRepository _listRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IMembersRepository _membersRepo;

    public TaskController(ITaskRepository taskRepo, IListRepository listRepo, IBoardRepository boardRepo, IMembersRepository membersRepo){
        _listRepo = listRepo;
        _taskRepo = taskRepo;
        _boardRepo = boardRepo;
        _membersRepo = membersRepo;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllTasks")]
    public async Task<IActionResult> GetAllTasks(){
        try{
            var tasks = await _taskRepo.GetAllTaskAsync();
            if(tasks.Count == 0){
                return NotFound("No tasks found");
            }

            var taskDto = tasks.Select(x => x.ToTaskDto());
            return Ok(taskDto);

        }catch(Exception e){
            return StatusCode(500, e.Message);
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTaskById")]
    public async Task<IActionResult> GetTaskById (int taskId){
        try{
            var task = await _taskRepo.GetTaskByIdAsync(taskId);
            if(task == null){
                return NotFound("Task Not Found!");
            }
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List Not Found!");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
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
            if (isMember || userTokenRole == "Admin")
            {
                return Ok(task.ToTaskDto());
            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal Server Error"+e.Message);
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTasksByWorkspaceId")]
    public async Task<IActionResult> GetTasksByWorkspaceId(int workspaceId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User Not Found!");
            }
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var tasks = await _taskRepo.GetTasksByWorkspaceIdAsync(workspaceId); // Await here
                 return Ok(tasks);
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error"+e.Message);
        }
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateTask")]
    public async Task<IActionResult> UpdateTask (UpdateTaskRequestDTO taskDto){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{
           
            var list = await _listRepo.GetListByIdAsync(taskDto.ListId);
            if (list == null)
            {
                return NotFound("List Not Found!");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
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
            if (isMember || userTokenRole == "Admin")
            {
                var taskModel = await _taskRepo.UpdateTaskAsync(taskDto);

                if (taskModel == null)
                {
                    return NotFound("Task not found");
                }

                return Ok(taskModel.ToTaskDto());
            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
        
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteTask")]
    public async Task<IActionResult> DeleteTask (int taskId){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{
                var task = await _taskRepo.GetTaskByIdAsync(taskId);
                if(task == null){
                    return NotFound("Task Not Found!");
                }
                var list = await _listRepo.GetListByIdAsync(task.ListId);
                if (list == null)
                {
                    return NotFound("List Not Found!");
                }

                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
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
                if (isMember || userTokenRole == "Admin")
                {
                    var taskModel = await _taskRepo.DeleteTaskAsync(taskId);
                    if (taskModel == null)
                    {
                        return NotFound("Task dose not exists");
                    }

                    return Ok("Task Deleted");
                }
                return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal Server Error "+e.Message);
        }

    }

    // relationship task and list 1-many
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateTask")]
    public async Task<IActionResult> CreateTask (CreateTaskRequestDTO taskDto){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        try
        {
            var list = await _listRepo.GetListByIdAsync(taskDto.ListId);
            if (list == null)
            {
                return NotFound("List not found!");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board not found!");
            }

            var workspaceId = board.WorkspaceId;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User Not Found!");
            }
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);

            if (isMember || userTokenRole == "Admin")
            {
                if (!await _listRepo.ListExists(taskDto.ListId))
                {
                    return BadRequest("List did not exist");

                }

                var taskModel = taskDto.ToTaskFromCreate();
                await _taskRepo.CreateTaskAsync(taskModel);
                return CreatedAtAction(nameof(GetTaskById), new { id = taskModel.TaskId }, taskModel.ToTaskDto());
            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, e.Message);
        }

    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet ("GetTaskByListId")]

    public async Task<IActionResult> GetTasksByListId (int listId){
        try{
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User Not Found!");
            }
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);

            if (isMember || userTokenRole == "Admin")
            {
                var tasks = await _taskRepo.GetTaskByListId(listId);

                if (tasks.Count == 0)
                {
                    return BadRequest("There are no Tasks!");
                }

                return Ok(tasks);
            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }

    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteTaskByListId")]

    public async Task<IActionResult> DeleteTaskByListId (ListIdDTO listIdDto){

        if(!await _listRepo.ListExists(listIdDto.ListId)){
            return StatusCode(404, "List Not Found");
        }

        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }


        try{
            var list = await _listRepo.GetListByIdAsync(listIdDto.ListId);
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User Not Found!");
            }
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            if (isMember || userTokenRole == "Admin")
            {

                var taskModel = await _taskRepo.DeleteTaskByListIdAsync(listIdDto.ListId);

                if (taskModel.Count == 0)
                {
                    return NotFound("Task not found");
                }

                return Ok("Task Deleted");

            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(404, "Internal Server Error!"+e.Message);
        }

    }


}