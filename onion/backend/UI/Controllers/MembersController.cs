using Application.Dtos.MembersDtos;
using Application.Services.InviteMembers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/members")]
public class MembersController : ControllerBase
{
    private readonly IInviteMembesService _inviteMembesService;

    public MembersController(IInviteMembesService inviteMembesService)
    {
        _inviteMembesService = inviteMembesService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllMembers")]
    public async Task<IActionResult> GetAllMembers()
    {
        try
        {
            var members = await _inviteMembesService.GetAllMembers();
            
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

            var members = await _inviteMembesService.GetAllMembersByWorkspace(workspaceId);

            return Ok(members);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateMember")]
    public async Task<IActionResult> UpdateMember(UpdateMemberDto updateMemberDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var member = _inviteMembesService.UpdateMember(updateMemberDto);

            return Ok(member);
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

            var member = await _inviteMembesService.RemoveMember(removeMemberDto);

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

            var member = await _inviteMembesService.DeleteMember(memberIdDto);
           
            return Ok(member);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

}