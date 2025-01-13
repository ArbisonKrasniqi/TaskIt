using Application.Dtos.MembersDtos;
using Application.Services.InviteMembers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/members")]
public class MembersController : ControllerBase
{
    private readonly IInviteMembersService _inviteMembersService;

    public MembersController(IInviteMembersService inviteMembersService)
    {
        _inviteMembersService = inviteMembersService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllMembers")]
    public async Task<IActionResult> GetAllMembers()
    {
        try
        {
            var members = await _inviteMembersService.GetAllMembers();
            
            return Ok(members);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetAllMembersByWorkspace")]
    public async Task<IActionResult> GetAllMembersByWorkspace(int workspaceId)
    {
        try
        {
            if (Int32.IsNegative(workspaceId)) return BadRequest("Workspace Id is invalid");

            var members = await _inviteMembersService.GetAllMembersByWorkspace(workspaceId);

            return Ok(members);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("RemoveMember")]
    public async Task<IActionResult> RemoveMember(RemoveMemberDto removeMemberDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var member = await _inviteMembersService.RemoveMember(removeMemberDto);

            return Ok(member);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteMember")]
    public async Task<IActionResult> DeleteMember(MemberIdDto memberIdDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var member = await _inviteMembersService.DeleteMember(memberIdDto);
           
            return Ok(member);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

}