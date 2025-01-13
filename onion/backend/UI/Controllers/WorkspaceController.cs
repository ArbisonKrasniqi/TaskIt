using Application.Dtos.WorkspaceDtos;
using Application.Services.Workspace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/workspace")]
public class WorkspaceController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;

    public WorkspaceController(IWorkspaceService workspaceService, IHttpContextAccessor httpContextAccessor)
    {
        _workspaceService = workspaceService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllWorkspaces")]
    public async Task<IActionResult> GetAllWorkspaces()
    {
        try
        {
            var workspaces = await _workspaceService.GetAllWorkspaces();
            return Ok(workspaces);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetWorkspaceById")]
    public async Task<IActionResult> GetWorkspaceById(int workspaceId)
    {
        try
        {
            
            if (Int32.IsNegative(workspaceId)) return BadRequest("Workspace Id is invalid");

            var workspace = await _workspaceService.GetWorkspaceById(workspaceId);

            return Ok(workspace);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetWorkspacesByMemberId")]
    public async Task<IActionResult> GetWorkspacesByMemberId(string memberId)
    {
        try
        {
         
            if (string.IsNullOrEmpty(memberId)) return BadRequest("Member Id is invalid");

            var workspaces = await _workspaceService.GetWorkspacesByMemberId(memberId);

            return Ok(workspaces);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateWorkspace")]
    public async Task<IActionResult> CreateWorkspace(CreateWorkspaceDto createWorkspaceDto)
    {
        try
        {
       
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var workspace = await _workspaceService.CreateWorkspace(createWorkspaceDto);
            return Ok(workspace);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateWorkspace")]
    public async Task<IActionResult> UpdateWorkspace(UpdateWorkspaceDto updateWorkspaceDto)
    {
        try
        {
           
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var workspace = await _workspaceService.UpdateWorkspace(updateWorkspaceDto);

            return Ok(workspace);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteWorkspace")]
    public async Task<IActionResult> DeleteWorkspace(WorkspaceIdDto workspaceIdDto)
    {
        try
        {
          
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var workspace = await _workspaceService.DeleteWorkspace(workspaceIdDto);

            return Ok(workspace);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }


}