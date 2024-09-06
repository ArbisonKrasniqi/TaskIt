using backend.DTOs.Workspace;
using backend.DTOs.WorkspaceActivity.Input;
using backend.DTOs.WorkspaceActivity.Output;
using backend.Interfaces;
using backend.Mappers.WorkspaceActivity;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

public class WorkspaceActivityController :ControllerBase
{
    private readonly IWorkspaceActivityRepository _workspaceActivityRepo;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly IUserRepository _userRepo;
    public WorkspaceActivityController(IWorkspaceActivityRepository workspaceActivityRepo,
        IWorkspaceRepository workspaceRepo, IMembersRepository membersRepo, IUserRepository userRepo)
    {
        _workspaceActivityRepo = workspaceActivityRepo;
        _workspaceRepo = workspaceRepo;
        _membersRepo = membersRepo;
        _userRepo = userRepo;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllWorkspaceActivities")]
    public async Task<IActionResult> GetAllWorkspaceActivities()
    {
        try
        {
            var workspaceActivities = await _workspaceActivityRepo.GetAllWorkspacesActivityAsync();
            if (workspaceActivities.Count == 0)
            {
                return Ok(new List<WorkspaceActivityDto>());
            }

            return Ok(workspaceActivities);
        }
        catch (Exception e)
        {
            return StatusCode(500, "There has been an internal server error! "+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetWorkspaceActivityById")]
    public async Task<IActionResult> GetWorkspaceActivityById(int id)
    {
        try
        {
            var workspaceActivity = await _workspaceActivityRepo.GetWorkspaceActivityByIdAsync(id);
            if (workspaceActivity == null)
            {
                return NotFound("Workspace Activity Not Found!");
            }
            
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(workspaceActivity.WorkspaceId);
            if (workspace == null)
            {
                return null;
            }

            return Ok(workspaceActivity);
        }  
        catch (Exception e)
        {
            return StatusCode(500, "There has been an internal server error! "+e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetWorkspaceActivityByWorkspaceId")]
    public async Task<IActionResult> GetWorkspaceActivityByWorkspaceId(WorkspaceIdDto workspaceIdDto)
    {
        try
        {
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(workspaceIdDto.WorkspaceId);
            if (workspace == null) return NotFound("Workspace Not Found!");
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (userId == null)
            {
                return NotFound("User Not Found!");
            }

            var isMember = await _membersRepo.IsAMember(userId, workspace.WorkspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var workspaceActivities = await _workspaceActivityRepo.GetWorkspaceActivitiesByWorkspace(workspaceIdDto.WorkspaceId);
                if (workspaceActivities.Count == 0)
                {
                    return Ok(new List<WorkspaceActivityDto>());
                }
                return Ok(workspaceActivities);
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error! "+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateWorkspaceActivity")]
    public async Task<IActionResult> CreateWorkspaceActivity(AddWorkspaceActivityDto workspaceActivityDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(workspaceActivityDto.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace Not Found!");
            }
            if (userId == null)
            {
                return NotFound("User Not Found!");
            }
            var isMember = await _membersRepo.IsAMember(userId, workspace.WorkspaceId);
            if (isMember || userTokenRole == "Admin")
            {
                var workspaceActivity = workspaceActivityDto.ToWorkspaceActivityFromCreate(userId);
                await _workspaceActivityRepo.CreateWorkspaceActivityAsync(workspaceActivity);
                return CreatedAtAction(nameof(GetWorkspaceActivityById),
                    new { id = workspaceActivity.WorkspaceActivityId }, workspaceActivity);
            }
            return StatusCode(401, "You are not authorized");
        }    
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error! "+e.Message);
        }
        
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteWorkspaceActivity")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteWorkspaceActivity(WorkspaceActivityIdDto workspaceActivityIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var workspaceActivity = await _workspaceActivityRepo.GetWorkspaceActivityByIdAsync(workspaceActivityIdDto.WorkspaceActivityId);
            if (workspaceActivity == null)
            {
                return NotFound("Workspace Activity Not Found!");
            }
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(workspaceActivity.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace Not Found!");
            }
             
            var workspaceActivityModel = await _workspaceActivityRepo.DeleteWorkspaceActivityByIdAsync(workspaceActivityIdDto.WorkspaceActivityId);
            if (workspaceActivityModel == null)
            {
                return NotFound("Workspace Activity Not Found!");
            }
         return Ok("workspace activity deleted!");
        } catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error! "+e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteWorkspaceActivityByWorkspaceId")]
    public async Task<IActionResult> DeleteWorkspaceActivityByWorkspaceId(WorkspaceIdDto workspaceIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(workspaceIdDto.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace Not Found!");
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (userId == null)
            {
                return NotFound("User Not Found!");
            }
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceIdDto.WorkspaceId);
            if (isOwner || userTokenRole == "Admin")
            {
                var workspaceActivities =
                    await _workspaceActivityRepo.DeleteWorkspaceActivitiesByWorkspace(workspaceIdDto.WorkspaceId);
                if (workspaceActivities.Count == 0)
                {
                    return Ok(new List<WorkspaceActivityDto>());
                }
                return Ok(workspaceActivities);
            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(404, "Internal Server Error! "+e.Message);
        }
    }
















}