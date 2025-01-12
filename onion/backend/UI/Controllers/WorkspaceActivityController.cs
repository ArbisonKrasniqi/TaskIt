using Application.Services.WorkspaceActivity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/workspaceActivity")]
public class WorkspaceActivityController : ControllerBase
{
    private readonly IWorkspaceActivityService _activityService;

    public WorkspaceActivityController(IWorkspaceActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet("GetActivityByWorkspaceId")]
    public async Task<IActionResult> GetActivityByWorkspaceId(int workspaceId)
    {
        try
        {
            if (int.IsNegative(workspaceId))
            {
                return BadRequest("WorkspaceId can not be negative");

            }

            var workspaceActivities = await _activityService.GetActivityByWorkspaceId(workspaceId);
            return Ok(workspaceActivities);

        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}