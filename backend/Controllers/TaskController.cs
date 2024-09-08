using System.Linq; 
using System.Threading.Tasks;
using AutoMapper;
using backend.DTOs.Task;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs.List;
using backend.DTOs.TaskMember.Output;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient.DataClassification;

namespace backend.Controllers;


[Route("backend/task")]
[ApiController]

public class  TaskController : ControllerBase{

    private readonly IListRepository _listRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILabelRepository _labelRepo;
    private readonly ITaskMemberRepository _taskMemberRepo;
    private readonly IMapper _mapper;
    
    public TaskController(IMapper mapper, ITaskMemberRepository taskMemberRepo, ITaskRepository taskRepo, IListRepository listRepo, IBoardRepository boardRepo, IMembersRepository membersRepo, IUserRepository userRepo, ILabelRepository labelRepo)
    {
        _mapper = mapper;
        _taskMemberRepo = taskMemberRepo;
        _listRepo = listRepo;
        _taskRepo = taskRepo;
        _boardRepo = boardRepo;
        _membersRepo = membersRepo;
        _userRepo = userRepo;
        _labelRepo = labelRepo;
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

            var taskDtos = new List<TaskDto>();

            foreach (var task in tasks)
            {
                // Get the labels for the current task
                var labels = await _labelRepo.GetLabelsByTaskId(task.TaskId);
                var taskMembers = await _taskMemberRepo.GetAllTaskMembersByTaskIdAsync(task.TaskId);
                var taskMembersDto = _mapper.Map<List<TaskMemberDto>>(taskMembers);
                // Convert the task to a DTO and add the labels
                var taskDto = task.ToTaskDto(labels, taskMembersDto);

                // Add the DTO to the list
                taskDtos.Add(taskDto);
            }

            return Ok(taskDtos);

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
                var taskLabels = await _labelRepo.GetLabelsByTaskId(task.TaskId);
                var taskMembers = await _taskMemberRepo.GetAllTaskMembersByTaskIdAsync(task.TaskId);
                return Ok(task.ToTaskDto(taskLabels, taskMembers));
            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal Server Error");
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

            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var tasks = await _taskRepo.GetTasksByWorkspaceIdAsync(workspaceId); // Await here
                
                var taskDtos = new List<TaskInfoDto2>();

                foreach (var task in tasks)
                {
                    // Get the labels for the current task
                    var labels = await _labelRepo.GetLabelsByTaskId(task.TaskId);
                    var taskMembers = await _taskMemberRepo.GetAllTaskMembersByTaskIdAsync(task.TaskId);
                    // Convert the task to a DTO and add the labels
                    var taskDto = task.toTaskInfoDto2(labels, taskMembers);

                    // Add the DTO to the list
                    taskDtos.Add(taskDto);
                }

                return Ok(taskDtos);
                
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
            var task = await _taskRepo.GetTaskByIdAsync(taskDto.TaskId);
            if(task == null){
                return NotFound("Task not found");
            }
           
            var list = await _listRepo.GetListByIdAsync(taskDto.ListId);
            if (list == null)
            {
                return StatusCode(404, "List Not Found");
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
                var taskModel = await _taskRepo.UpdateTaskAsync(taskDto);
                if (taskModel == null)
                {
                    return NotFound("Task not found");
                }
                

                var taskLabels = await _labelRepo.GetLabelsByTaskId(taskModel.TaskId);
                return Ok(taskModel.ToTaskDtoLabels(taskLabels));
            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal Server Error!");
        }
        
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteTask")]
    public async Task<IActionResult> DeleteTask (TaskIdDTO taskIdDto){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{
                var task = await _taskRepo.GetTaskByIdAsync(taskIdDto.TaskId);
                if(task == null){
                    return NotFound("Task not found");
                }
                
                var list = await _listRepo.GetListByIdAsync(task.ListId);
                if (list == null)
                {
                    return StatusCode(404, "List Not Found");
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
                    var taskModel = await _taskRepo.DeleteTaskAsync(taskIdDto.TaskId);
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
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);

            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            if (isMember || userTokenRole == "Admin")
            {
                if (!await _listRepo.ListExists(taskDto.ListId))
                {
                    return BadRequest("List did not exist");

                }

                var taskModel = taskDto.ToTaskFromCreate();
                await _taskRepo.CreateTaskAsync(taskModel);
                var labels = new List<Models.Label>();
                var taskMembers = new List<TaskMemberDto>();
                return CreatedAtAction(nameof(GetTaskById), new { id = taskModel.TaskId }, taskModel.ToTaskDto(labels,taskMembers));
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
                return NotFound("List does not exist!");
            }
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board not found!");
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
                var tasks = await _taskRepo.GetTaskByListId(listId);

                if (tasks.Count == 0)
                {
                    return NotFound("There are no Tasks!");
                }

                var taskDtos = new List<TaskDto>();

                foreach (var task in tasks)
                {
                    // Get the labels for the current task
                    var labels = await _labelRepo.GetLabelsByTaskId(task.TaskId);
                    var taskMembers = await _taskMemberRepo.GetAllTaskMembersByTaskIdAsync(task.TaskId);
                    var taskMembersDto = _mapper.Map<List<TaskMemberDto>>(taskMembers);
                    // Convert the task to a DTO and add the labels
                    var taskDto = task.ToTaskDto(labels, taskMembersDto);

                    // Add the DTO to the list
                    taskDtos.Add(taskDto);
                }

                return Ok(taskDtos);
                
            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal Server Error!");
        }

    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteTaskByListId")]

    public async Task<IActionResult> DeleteTaskByListId (ListIdDTO listIdDTO){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        
        try{
            var list = await _listRepo.GetListByIdAsync(listIdDTO.ListId);
            if (list == null)
            {
                return NotFound("List not found");
            }
            
            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board not found!");
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

                var taskModel = await _taskRepo.DeleteTaskByListIdAsync(listIdDTO.ListId);

                if (taskModel.Count == 0)
                {
                    return NotFound("Task not found");
                }

                return Ok("Task Deleted");

            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(404, "Internal Server Error!");
        }

    }


}