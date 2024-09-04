using AutoMapper;
using backend.DTOs.TaskMember.Input;
using backend.DTOs.TaskMember.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;
[Route("backend/TaskMembers")]
[ApiController]
public class TaskMemberController : ControllerBase
{
    private readonly ITaskMemberRepository _taskMemberRepo;
    private readonly IMapper _mapper;
    private readonly ITaskRepository _taskRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMembersRepository _memberRepo;
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IWorkspaceRepository _workspaceRepo;

    public TaskMemberController(ITaskMemberRepository taskMemberRepo,
                                IMapper mapper,
                                ITaskRepository taskRepo,
                                IUserRepository userRepo,
                                IMembersRepository memberRepo,
                                IListRepository listRepo,
                                IBoardRepository boardRepo,
                                IWorkspaceRepository workspaceRepo)
    {
        _taskMemberRepo = taskMemberRepo;
        _mapper = mapper;
        _taskRepo = taskRepo;
        _userRepo = userRepo;
        _memberRepo = memberRepo;
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _workspaceRepo = workspaceRepo;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllTaskMembers")]
    public async Task<IActionResult> GetAllTaskMembers()
    {
        try
        {
            var taskMembers = await _taskMemberRepo.GetAllTaskMembersAsync();
            if (taskMembers.Count == 0)
            {
                return Ok(new List<TaskMemberDto>());
            }

            var taskMembersDto = _mapper.Map<List<TaskMemberDto>>(taskMembers);
            return Ok(taskMembersDto);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetTaskMemberById")]
    public async Task<IActionResult> GetTaskMemberById(int taskMemberId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var taskMember = await _taskMemberRepo.GetTaskMemberByIdAsync(taskMemberId);

            var taskId = taskMember.TaskId;
            var isTaskMember = userId != null && await _taskMemberRepo.IsATaskMember(userId, taskId);

            if (isTaskMember || userTokenRole == "Admin")
            {
                var taskMemberDto = _mapper.Map<TaskMemberDto>(taskMember);
                return Ok(taskMemberDto);
            }

            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetAllTaskMembersByTaskId")]
    public async Task<IActionResult> GetAllTaskMembersByTaskId(int taskId)
    {
        try
        {
            if (!await _taskRepo.TaskExists(taskId))
            {
                return NotFound("Task Not Found!");
            }

            var task = await _taskRepo.GetTaskByIdAsync(taskId);

            var list = await _listRepo.GetListByIdAsync(task.ListId);

            if (list == null)
            {
                return NotFound("List Not Found!");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);

            if (board == null)
            {
                return NotFound("Board Not Found!");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);

            if (workspace == null)
            {
                return NotFound("Workspace Not Found!");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = userId != null && await _memberRepo.IsAMember(userId, workspace.WorkspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var taskMembers = await _taskMemberRepo.GetAllTaskMembersByTaskIdAsync(taskId);

                if (taskMembers.Count == 0)
                {
                    return Ok(new List<TaskMemberDto>());
                }

                var taskMemberDto = _mapper.Map<IEnumerable<TaskMemberDto>>(taskMembers);
                return Ok(taskMemberDto);
            }

            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("AddTaskMember")]
    public async Task<IActionResult> AddTaskMember(AddTaskMemberDto addTaskMemberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (!await _taskRepo.TaskExists(addTaskMemberDto.TaskId))
            {
                return NotFound("Task Not Found!");
            }

            if (!await _userRepo.UserExists(addTaskMemberDto.UserId))
            {
                return NotFound("User Not Found!");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var task = await _taskRepo.GetTaskByIdAsync(addTaskMemberDto.TaskId);
            if (task == null)
            {
                return NotFound("Task Not Found!");
            }
            
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List Not Found!");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board Not Found!");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace Not Found");
            }
            var isMember = userId != null && await _memberRepo.IsAMember(userId, workspace.WorkspaceId);

            var memberAddedIsMember = await _memberRepo.IsAMember(addTaskMemberDto.UserId, workspace.WorkspaceId);

            if (!memberAddedIsMember)
            {
                return NotFound("The User is not a member of this workspace!");
            }

            var memberAddedAlreadyATaskMember = await _taskMemberRepo.IsATaskMember(addTaskMemberDto.UserId, addTaskMemberDto.TaskId);
            if (memberAddedAlreadyATaskMember)
            {
                return BadRequest("User is already a member in this task!");
            }
            
            if (isMember || userTokenRole == "Admin")
            {
                await _taskMemberRepo.AddTaskMemberAsync(addTaskMemberDto);
                return StatusCode(200, "Member added to Task!");
            }

            return StatusCode(401, "You are not authorized!");


        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!" + e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateTaskMember")]
    public async Task<IActionResult> UpdateTaskMember(UpdateTaskMemberDto updateTaskMemberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (!await _taskRepo.TaskExists(updateTaskMemberDto.TaskId))
            {
                return NotFound("Task Not Found!");
            }

            if (!await _userRepo.UserExists(updateTaskMemberDto.UserId))
            {
                return NotFound("User Not Found!");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var task = await _taskRepo.GetTaskByIdAsync(updateTaskMemberDto.TaskId);
            if (task == null)
            {
                return NotFound("Task Not Found!");
            }
            
            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List Not Found!");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board Not Found!");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace Not Found");
            }
            
            var isMember = userId != null && await _memberRepo.IsAMember(userId, workspace.WorkspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var taskMemberModel = await _taskMemberRepo.UpdateTaskMemberAsync(updateTaskMemberDto);
                if (taskMemberModel == null)
                {
                   return NotFound("Task Not Found!");
                }

                var updatedTaskMemberDto = _mapper.Map<TaskMemberDto>(taskMemberModel);
                return Ok(updatedTaskMemberDto);   
            }

            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!" + e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("RemoveTaskMember")]
    public async Task<IActionResult> RemoveTaskMember([FromQuery] RemoveTaskMemberDto removeTaskMemberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (!await _taskRepo.TaskExists(removeTaskMemberDto.TaskId))
            {
                return NotFound("Task Not Found!");
            }

            if (!await _userRepo.UserExists(removeTaskMemberDto.UserId))
            {
                return NotFound("User Not Found!");
            }
            
            var task = await _taskRepo.GetTaskByIdAsync(removeTaskMemberDto.TaskId);

            var list = await _listRepo.GetListByIdAsync(task.ListId);

            if (list == null)
            {
                return NotFound("List Not Found!");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);

            if (board == null)
            {
                return NotFound("Board Not Found!");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);

            if (workspace == null)
            {
                return NotFound("Workspace Not Found!");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _memberRepo.IsAMember(userId, workspace.WorkspaceId);
            var isTaskMember = await _taskMemberRepo.IsATaskMember(removeTaskMemberDto.UserId, removeTaskMemberDto.TaskId);
            if (isMember || userTokenRole == "Admin")
            {
                if (isTaskMember)
                {
                    await _taskMemberRepo.RemoveTaskMemberAsync(removeTaskMemberDto.TaskId, removeTaskMemberDto.UserId);

                    return Ok("Member removed from Task");
                }

                return BadRequest("User is not a Task member!");
            }

            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!"+e.Message);
        }
    }

    [HttpDelete("DeleteMember")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteTaskMemberById([FromQuery] TaskMemberIdDto taskMemberIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var taskMemberModel = await _taskMemberRepo.DeleteTaskMemberByIdAsync(taskMemberIdDto.TaskMemberId);
            if (taskMemberModel == null)
            {
                return NotFound("TaskMember Not Found!");
            }

            return Ok("Member deleted from Task");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!");
        }
    }
    
    
}