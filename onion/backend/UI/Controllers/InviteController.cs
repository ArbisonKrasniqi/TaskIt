using Application.Dtos.InviteDtos;
using Application.Services.InviteMembers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/invite")]
public class InviteController : ControllerBase
{
    private readonly IInviteMembesService _inviteMembesService;

    public InviteController(IInviteMembesService inviteMembesService)
    {
        _inviteMembesService = inviteMembesService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllInvites")]
    public async Task<IActionResult> GetAllInvites()
    {
        try
        {
            var invites = await _inviteMembesService.GetAllInvites();
            return Ok(invites);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetInvitesByWorkspace")]
    public async Task<IActionResult> GetInvitesByWorkspace(int workspaceId)
    {
        try
        {
            if (Int32.IsNegative(workspaceId)) return BadRequest("Workspace Id is invalid");

            var invites = await _inviteMembesService.GetInvitesByWorkspace(workspaceId);

            return Ok(invites);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("CheckPendingInvite")]
    public async Task<IActionResult> CheckPendingInvite(CreateInviteDto createInviteDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var inviteExists = await _inviteMembesService.CheckPendingInvite(createInviteDto);

            return Ok(inviteExists);

        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("Invite")]
    public async Task<IActionResult> Invite(CreateInviteDto createInviteDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var invite = await _inviteMembesService.Invite(createInviteDto);

            return Ok(invite);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateInviteStatus")]
    public async Task<IActionResult> UpdateInviteStatus(UpdateInviteDto updateInviteDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var invite = await _inviteMembesService.UpdateInviteStatus(updateInviteDto);

            return Ok(invite);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateInvite")]
    public async Task<IActionResult> UpdateInvite(UpdateInviteAdminDto updateInviteAdminDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var invite = await _inviteMembesService.UpdateInvite(updateInviteAdminDto);

            return Ok(invite);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteInviteById")]
    public async Task<IActionResult> DeleteInviteById(InviteIdDto inviteIdDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var invite = await _inviteMembesService.DeleteInviteById(inviteIdDto);

            return Ok(invite);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    



}