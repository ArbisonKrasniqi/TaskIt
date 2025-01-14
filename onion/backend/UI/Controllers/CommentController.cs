using Application.Dtos.CommentDtos;
using Application.Services.Comment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;
[ApiController]
[Route("backend/comment")]
public class CommentController : ControllerBase
{
    private ICommentService _commentService;
    
    private readonly string? _userId;
    private readonly string? _userTokenRole;
    
    public CommentController(ICommentService commentService, IHttpContextAccessor httpContextAccessor)
    {
        _commentService = commentService;

        var user = httpContextAccessor.HttpContext?.User;
        _userId = user?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        _userTokenRole = user?.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
    }
    
    [HttpGet("GetCommentsByTaskId")]
    public async Task<IActionResult> GetCommentByTaskId(int taskId)
    {
        try
        {
            if (int.IsNegative(taskId))
            {
                return BadRequest("Task Id can not be negative");
            }

            var comment = await _commentService.GetCommentByTaskId(taskId);
            return Ok(comment);

        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
    [HttpPost("CreateComment")]
    public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentService.CreateComment(createCommentDto);
            return Ok(comment);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
    
    [HttpDelete("DeleteComment")]
    public async Task<IActionResult> DeleteComment(CommentIdDto commentIdDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentService.DeleteComment(commentIdDto);
            return Ok(comment);

        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("UpdateComment")]
    public async Task<IActionResult> UpdateComment(UpdateCommentDto updateCommentDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentService.UpdateComment(updateCommentDto);
            return Ok(comment);

        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}