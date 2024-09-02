using backend.DTOs.Comment;
using backend.Interfaces;
using backend.Mappers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace backend.Controllers;

[Route("backend/comment")]
[ApiController]

public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly ITaskRepository _taskRepo;
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepo;
    
    public CommentController(ICommentRepository commentRepo, ITaskRepository taskRepo, IListRepository listRepo, IBoardRepository boardRepo, IWorkspaceRepository workspaceRepo, IMembersRepository membersRepo, UserManager<User> userManager, IUserRepository userRepo)
    {
        _commentRepo = commentRepo;
        _taskRepo = taskRepo;
        _listRepo = listRepo;
        _boardRepo = boardRepo;
        _workspaceRepo = workspaceRepo;
        _membersRepo = membersRepo;
        _userManager = userManager;
        _userRepo = userRepo;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllComments")]
    public async Task<IActionResult> GetAllComments()
    {
        try
        {
            var comments = await _commentRepo.GetAllCommentsAsync();
            if (comments.Count == 0)
            {
                return NotFound("No comments found");
            }
            
            return Ok(comments);
        }
        catch (Exception e)
        {
            return StatusCode(500, "There has been an internal server error");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetCommentById")]
    public async Task<IActionResult> GetCommentById(int commentId)
    {
        try
        {
            var comment = await _commentRepo.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            var task = await _taskRepo.GetTaskByIdAsync(comment.TaskId);
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
                return Ok(comment);
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetCommentsByTaskId")]
    public async Task<IActionResult> GetCommentsByTaskId(int taskId)
    {
        try
        {
            var task = await _taskRepo.GetTaskByIdAsync(taskId);
            if (task == null) return NotFound("Task not found");

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null) return NotFound("List not found");

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null) return NotFound("Board not found");

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null) return NotFound("Workspace not found");
            
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
                var comments = await _commentRepo.GetCommentsByTaskIdAsync(taskId);
                if (comments == null) return NotFound("No comments");


                return Ok(comments);
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e) 
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetCommentsByUserId")]
    public async Task<IActionResult> GetCommentsByUserId(string userId)
    {
        if (!ModelState.IsValid) return BadRequest("Id cannot be empty");

        try
        {
            var userTokenId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            if (userTokenId == userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound("User not found");

                var comments = await _commentRepo.GetCommentsByUserIdAsync(userId);
                if (comments == null) return NotFound("No comments");

                return Ok(comments);
            }

            if (userTokenRole == "Admin")
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound("User not found");

                var comments = await _commentRepo.GetCommentsByUserIdAdminAsync(userId);
                if (comments == null) return NotFound("No comments");
                return Ok(comments);
            }
            
            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateComment")]
    public async Task<IActionResult> CreateComment(CreateCommentRequestDTO commentRequestDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var task = await _taskRepo.GetTaskByIdAsync(commentRequestDto.TaskId);
            if (task == null)
            {
                return NotFound("Task not found!");
            }

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null)
            {
                return NotFound("List not found!");
            }

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null)
            {
                return NotFound("Board not found!");
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
                var commentModel = commentRequestDto.ToCommentFromCreate(userId);
                await _commentRepo.CreateCommentAsync(commentModel);
                return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.CommentId }, commentModel);
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteComment")]
    public async Task<IActionResult> DeleteComment(CommentIdDTO commentIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var comment = await _commentRepo.GetCommentByIdAsync(commentIdDto.CommentId);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            var task = await _taskRepo.GetTaskByIdAsync(comment.TaskId);
            if (task == null) return NotFound("Task not found");


            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null) return NotFound("List not found");

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null) return NotFound("Board not found");

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null) return NotFound("Workspace not found");
            
            
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
                var commentModel = await _commentRepo.DeleteCommentAsync(commentIdDto.CommentId);
                if (commentModel == null)
                {
                    return NotFound("Comment does not exist");
                }

                return Ok("Comment deleted");
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateComment")]
    public async Task<IActionResult> UpdateComment(UpdateCommentDTO updateCommentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var comment = await _commentRepo.GetCommentByIdAsync(updateCommentDto.CommentId);
            if (comment == null) return NotFound("Comment not found");

            var task = await _taskRepo.GetTaskByIdAsync(comment.TaskId);
            if (task == null) return NotFound("Task not found");

            var list = await _listRepo.GetListByIdAsync(task.ListId);
            if (list == null) return NotFound("List not found");

            var board = await _boardRepo.GetBoardByIdAsync(list.BoardId);
            if (board == null) return NotFound("Board not found");

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null) return NotFound("Workspace not found");

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
                var commentModel = await _commentRepo.UpdateCommentAsync(updateCommentDto);
                if (commentModel == null) return NotFound("Comment not found");

                return Ok(commentModel.toCommentDto());
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }


}