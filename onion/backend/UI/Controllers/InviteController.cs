using Application.Dtos.InviteDtos;
using Application.Services.InviteMembers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/invite")]
public class InviteController : ControllerBase
{
    private readonly IInviteMembersService _inviteMembersService;

    public InviteController(IInviteMembersService inviteMembersService)
    {
        _inviteMembersService = inviteMembersService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllInvites")]
    public async Task<IActionResult> GetAllInvites()
    {
        try
        {
            var invites = await _inviteMembersService.GetAllInvites();
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

            var invites = await _inviteMembersService.GetInvitesByWorkspace(workspaceId);

            return Ok(invites);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("CheckPendingInvite")]
    public async Task<IActionResult> CheckPendingInvite(int workspaceId, string inviterId, string inviteeId)
    {
        try
        {
            var inviteDto = new CreateInviteDto(workspaceId, inviterId, inviteeId);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var inviteExists = await _inviteMembersService.CheckPendingInvite(inviteDto);

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

            var invite = await _inviteMembersService.Invite(createInviteDto);

            return Ok(invite);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateInviteStatus")]
    public async Task<IActionResult> UpdateInviteStatus(UpdateInviteDto updateInviteDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var invite = await _inviteMembersService.UpdateInviteStatus(updateInviteDto);

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

            var invite = await _inviteMembersService.UpdateInvite(updateInviteAdminDto);

            return Ok(invite);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteInviteById")]
    public async Task<IActionResult> DeleteInviteById(InviteIdDto inviteIdDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var invite = await _inviteMembersService.DeleteInviteById(inviteIdDto);

            return Ok(invite);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    



}