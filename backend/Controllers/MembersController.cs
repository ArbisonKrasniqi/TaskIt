using AutoMapper;
using backend.DTOs.Members;
using backend.DTOs.Members.Output;
using backend.Interfaces;

using Microsoft.AspNetCore.Authorization;
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
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;

    public MembersController(IMembersRepository userWorkspaceRepo, IWorkspaceRepository workspaceRepo, IMapper mapper, IUserRepository userRepo)
    {
        _membersRepo = userWorkspaceRepo;
        _workspaceRepo = workspaceRepo;
        _mapper = mapper;
        _userRepo = userRepo;
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("AddMember")]
    public async Task<IActionResult> AddMember(AddMemberDto addMemberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            if (!await _workspaceRepo.WorkspaceExists(addMemberDto.WorkspaceId))
            {
                return NotFound("Workspace Not found!");
            }

            if (!await _userRepo.UserExists(addMemberDto.UserId))
            {
                return NotFound("User does not exists!");
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, addMemberDto.WorkspaceId);
            if (ownsWorkspace || userTokenRole == "Admin")
            {
                await _membersRepo.AddMemberAsync(addMemberDto);
                return StatusCode(200, "Member added!");
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error: "+e.Message);
        }
        
    }
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("getAllMembers")]
    public async Task<IActionResult> GetAllMembers(int workspaceId)
    {
        try
        { 
            if (!await _workspaceRepo.WorkspaceExists(workspaceId))
            {
                return NotFound("Workspace Not found!");
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _membersRepo.IsAMember(userId, workspaceId);
            if (isMember || userTokenRole == "Admin")
            {

                var members = await _membersRepo.GetAllMembersAsync(workspaceId);

                if (members.Count() == 0) return Ok(new List<MemberDto>());

                var memberDtos = _mapper.Map<IEnumerable<MemberDto>>(members);
                return Ok(memberDtos);
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error: " + e.Message);
        }
    }
    
    

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("RemoveMember")]
    public async Task<IActionResult> RemoveMember([FromBody] RemoveMemberDto removeMemberDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            if (!await _workspaceRepo.WorkspaceExists(removeMemberDto.WorkspaceId))
            {
                return NotFound("Workspace Not found!");
            }

            if (!await _userRepo.UserExists(removeMemberDto.UserId))
            {
                return NotFound("User does not exists!");
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, removeMemberDto.WorkspaceId);
            if (ownsWorkspace || userTokenRole == "Admin")
            {
                var result = await _membersRepo.RemoveMemberAsync(removeMemberDto.WorkspaceId, removeMemberDto.UserId);
                if (result == null)
                {
                    return StatusCode(500, "User could not removed");
                }

                return Ok("Member removed!");
            }
            return StatusCode(401, "You are not authorized!");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error: "+ e.Message);  
        }
    }
}