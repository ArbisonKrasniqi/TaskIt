using AutoMapper;
using backend.DTOs.TaskLabel.Output;
using backend.DTOs.TaskLabelDTO.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("backend/taskLabel")]
[ApiController]
public class TaskLabelController : ControllerBase
{
    private readonly ITaskLabelRepository _taskLabelRepo;
    private readonly ILabelRepository _labelRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly IBoardActivityRepository _boardActivityRepo;

    public TaskLabelController(ITaskLabelRepository taskLabelRepo, ILabelRepository labelRepo, ITaskRepository taskRepo,
        IListRepository listRepo, IBoardRepository boardRepo,
        IWorkspaceRepository workspaceRepo, IUserRepository userRepo, IMapper mapper, IBoardActivityRepository boardActivityRepo)
    {
        _taskLabelRepo = taskLabelRepo;
        _labelRepo = labelRepo;
        _taskRepo = taskRepo;
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _workspaceRepo = workspaceRepo;
        _userRepo = userRepo;
        _mapper = mapper;
        _boardActivityRepo = boardActivityRepo;
    }

    [HttpGet("GetAllTaskLabels")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllTaskLabels()
    {
        try
        {
            var taskLabels = await _taskLabelRepo.getAllTaskLabelsAsync();
            var taskLabelsDto = _mapper.Map<List<TaskLabelDTO>>(taskLabels);
            return Ok(taskLabelsDto);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error"+e.Message);
        }
    }

    [HttpGet("GetTaskLabelById")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetTaskLabelById(int id)
    {
        try
        {
            var taskLabel = await _taskLabelRepo.getTaskLabelByIdAsync(id);
            return Ok(taskLabel);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("AssignLabelToTask")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> AssignLabelToTask(AssignLabelDTO assignLabelDto){
        try
        {
            var taskLabelExists = await _taskLabelRepo.TaskLabelExists(assignLabelDto);
            if (taskLabelExists)
            {
                return StatusCode(409, "Label is already assigned");
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var label = await _labelRepo.GetLabelByIdAsync(assignLabelDto.LabelId);
            if (label == null) return NotFound("Label not found");

            var labelBoard = await _boardRepo.GetBoardByIdAsync(label.BoardId);
            if (labelBoard == null) return NotFound("Label board not found");

            var task = await _taskRepo.GetTaskByIdAsync(assignLabelDto.TaskId);
            if (task == null) return NotFound("Task not found");

            var taskList = await _listRepo.GetListByIdAsync(task.ListId);
            if (taskList == null) return NotFound("List not found");

            var taskBoard = await _boardRepo.GetBoardByIdAsync(taskList.BoardId);
            if (taskBoard == null) return NotFound("Task board not found");

            if (labelBoard != taskBoard) return StatusCode(403, "Label and Task aren't in the same board");

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(taskBoard.WorkspaceId);
            if (workspace == null) return NotFound("Workspace not found");

            var isMember = await _userRepo.UserIsMember(userId, workspace.WorkspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            var isAdmin = userTokenRole == "Admin";

            if (isOwner || isAdmin)
            {
                var taskLabelModel = _mapper.Map<TaskLabel>(assignLabelDto);
                var createdTaskLabel = await _taskLabelRepo.assignLabelToTask(taskLabelModel);
                var createdTaskLabeldto = _mapper.Map<TaskLabelDTO>(createdTaskLabel);
                return Ok(createdTaskLabeldto);
            }

            if (isMember)
            {
                if (taskBoard.IsClosed)
                {
                    return StatusCode(401, "Board is closed");
                }
                var taskLabelModel = _mapper.Map<TaskLabel>(assignLabelDto);
                var createdTaskLabel = await _taskLabelRepo.assignLabelToTask(taskLabelModel);
                var createdTaskLabeldto = _mapper.Map<TaskLabelDTO>(createdTaskLabel);

                //Assign BoardActivity
                    var boardActivity = new BoardActivity{
                        BoardId = taskLabelModel.TaskLabelId,
                        UserId = userId,
                        ActionType = "assign",
                        EntityName = "label " + assignLabelDto.TaskId,
                        ActionDate = DateTime.Now
                    };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);

                return Ok(createdTaskLabeldto);
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error"+e);
        }
    }

    [HttpDelete("RemoveLabelFromTask")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> RemoveLabelFromTask(AssignLabelDTO assignLabelDto)
    {
        try
        {
            var taskLabelExists = await _taskLabelRepo.TaskLabelExists(assignLabelDto);
            if (!taskLabelExists)
            {
                return NotFound("Label is not assigned to task");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var label = await _labelRepo.GetLabelByIdAsync(assignLabelDto.LabelId);
            if (label == null) return NotFound("Label not found");

            var labelBoard = await _boardRepo.GetBoardByIdAsync(label.BoardId);
            if (labelBoard == null) return NotFound("Label board not found");

            var task = await _taskRepo.GetTaskByIdAsync(assignLabelDto.TaskId);
            if (task == null) return NotFound("Task not found");

            var taskList = await _listRepo.GetListByIdAsync(task.ListId);
            if (taskList == null) return NotFound("List not found");

            var taskBoard = await _boardRepo.GetBoardByIdAsync(taskList.BoardId);
            if (taskBoard == null) return NotFound("Task board not found");

            if (labelBoard != taskBoard) return StatusCode(403, "Label and Task aren't in the same board");

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(taskBoard.WorkspaceId);
            if (workspace == null) return NotFound("Workspace not found");

            var isMember = await _userRepo.UserIsMember(userId, workspace.WorkspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            var isAdmin = userTokenRole == "Admin";

            if (isOwner || isAdmin)
            {
                var taskLabelModel =
                    await _taskLabelRepo.getTaskLabelByLabelAndTask(assignLabelDto.LabelId, assignLabelDto.TaskId);
                var removedTaskLabel = await _taskLabelRepo.removeTaskLabel(taskLabelModel);
                return Ok("Deleted task label");
            }

            if (isMember)
            {
                if (taskBoard.IsClosed)
                {
                    return StatusCode(401, "Board is closed");
                }
                var taskLabelModel =
                    await _taskLabelRepo.getTaskLabelByLabelAndTask(assignLabelDto.LabelId, assignLabelDto.TaskId);
                await _taskLabelRepo.removeTaskLabel(taskLabelModel);



                //Remove BoardActivity
                    var boardActivity = new BoardActivity{
                        BoardId = taskLabelModel.TaskLabelId,
                        UserId = userId,
                        ActionType = "remove",
                        EntityName = "label " + assignLabelDto.TaskId,
                        ActionDate = DateTime.Now
                    };
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);
                
                return Ok("Deleted task label");
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
        
    }
    

}