using backend.DTOs.Members;
using backend.DTOs.User.Input;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
namespace backend.Controllers;
[Route("backend/Members")]
//means that the controller or action method will be accessible using
//an HTTP request to http://yourdomain/backend/workspace
[ApiController]
public class MembersController: ControllerBase
{
    private readonly IMembersRepository _membersRepo; //qe me kon immutable
    private readonly IWorkspaceRepository _workspaceRepo;

    public MembersController(IMembersRepository userWorkspaceRepo, IWorkspaceRepository workspaceRepo)
    {
        _membersRepo = userWorkspaceRepo;
        _workspaceRepo = workspaceRepo;
    }

    [HttpPost]
    [Route("AddMember")]
    public async Task<IActionResult> AddMember(AddMemberDto addMemberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _membersRepo.AddMemberAsync(addMemberDto);
            return StatusCode(200, "Member added!");

        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error: "+e.Message);
        }
        
    }
    [HttpGet]
    [Route("getAllMembers")]
    public async Task<IActionResult> GetAllMembers(int workspaceId)
    {
        try
        {
            var members = await _membersRepo.GetAllMembersAsync(workspaceId);
            return Ok(members);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error: " + e.Message);
        }
    }
    
    


    [HttpDelete]
    [Route("RemoveMember")]
    public async Task<IActionResult> RemoveMember([FromBody] RemoveMemberDto removeMemberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _membersRepo.RemoveMemberAsync(removeMemberDto.WorkspaceId, removeMemberDto.UserId);
            if (result == null)
            {
                return StatusCode(500, "User could not removed");
            }
            return Ok("Member removed!");

        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error: "+ e.Message);  
        }
    }
}