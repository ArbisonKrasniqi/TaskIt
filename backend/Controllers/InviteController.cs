using AutoMapper;
using backend.DTOs.Invite.Input;
using backend.DTOs.Invite.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{

    [Route("backend/invite")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly IInviteRepository _inviteRepo;
        private readonly IWorkspaceRepository _workspaceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public InviteController(IInviteRepository inviteRepo, IWorkspaceRepository workspaceRepo, IUserRepository userRepo,IMapper mapper)
        {
            _inviteRepo = inviteRepo;
            _workspaceRepo = workspaceRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet(template: "GetAllInvites")]
        public async Task<IActionResult> GetAllInvites()
        {
            try
            {
                var invites = await _inviteRepo.GetAllInvitesAsync();
                if (invites.Count() == 0) return Ok(new List<InviteDto>());
              

                var invitesDtos = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                return Ok(invitesDtos);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetInviteById")]
        public async Task<IActionResult> GetInviteById(int id)
        {
            try
            {
                var invite = await _inviteRepo.GetInviteByIdAsync(id);
                if (invite == null) return NotFound("Invite Not Found!");
          
                var inviteDto = _mapper.Map<InviteDtoOut>(invite);
                return Ok(inviteDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetInvitesByInviter")]
        public async Task<IActionResult> GetInvitesByInviterId(string inviterId)
        {
            try
            {
                if (!await _userRepo.UserExists(inviterId))
                {
                    return BadRequest("Inviter does not exists!");
                }
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                if (userId == inviterId || userTokenRole == "Admin")
                {
                    var invites = await _inviteRepo.GetInvitesByInviterAsync(inviterId);
                    if (invites.Count() == 0) return Ok(new List<InviteDto>());
             
                    var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                    return Ok(invitesDto);   
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetInvitesByInvitee")]
        public async Task<IActionResult> GetInvitesByInviteeId(string inviteeId)
        {
            try
            {
                if (!await _userRepo.UserExists(inviteeId))
                {
                    return BadRequest("Inviter does not exists!");
                }
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                if (userId == inviteeId || userTokenRole == "Admin")
                {
                    var invites = await _inviteRepo.GetInvitesByInviteeAsync(inviteeId);
                    if (invites.Count() == 0) return Ok(new List<InviteDto>());


                    var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                    return Ok(invitesDto);
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);

            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetInvitesByWorkspace")]
        public async Task<IActionResult> GetInvitesByWorkspace(int workspaceId)
        {
            try
            {
                if (!await _workspaceRepo.WorkspaceExists(workspaceId))
                {
                    return BadRequest("Workspace not found!");
                }
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
                if (ownsWorkspace || userTokenRole == "Admin")
                {
                    var invites = await _inviteRepo.GetInvitesByWorkspaceAsync(workspaceId);
                    if (invites.Count() == 0) return Ok(new List<InviteDto>());
                    var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                    return Ok(invitesDto);    
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetPendingInvitesByInviter")]
        public async Task<IActionResult> GetPendingInvitesByInviter(string inviterId)
        {
            try
            {
                if (!await _userRepo.UserExists(inviterId))
                {
                    return BadRequest("User not found!");
                }
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                if (userId == inviterId || userTokenRole == "Admin")
                {
                    var invites = await _inviteRepo.GetPendingInvitesByInviter(inviterId);
                    if (invites.Count() == 0) return Ok(new List<InviteDto>());
                    var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                    return Ok(invitesDto);
                }
                return StatusCode(401, "You are not authorized!");
            }  catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetPendingInvitesByInvitee")]
        public async Task<IActionResult> GetPendingInvitesByInvitee(string inviteeId)
        {
            try
            {
                if (!await _userRepo.UserExists(inviteeId))
                {
                    return BadRequest("User not found!");
                }
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                if (userId == inviteeId || userTokenRole == "Admin")
                {
                    var invites = await _inviteRepo.GetPendingInvitesByInvitee(inviteeId);
                    if (invites.Count() == 0) return Ok(new List<InviteDto>());
                    var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                    return Ok(invitesDto);
                }
                return StatusCode(401, "You are not authorized!");
            }  catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Invite")]
        public async Task<IActionResult> Invite(InviteDto inviteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _workspaceRepo.WorkspaceExists(inviteDto.WorkspaceId))
            {
                return BadRequest("Workspace not Found!");
            }

            if (!await _userRepo.UserExists(inviteDto.InviterId))
            {
                return BadRequest("Inviter Not Found!");
            }
            if (!await _userRepo.UserExists(inviteDto.InviteeId))
            {
                return BadRequest("Invitee Not Found!");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(inviteDto.WorkspaceId);
            if (workspace.OwnerId != inviteDto.InviterId)
            {
                return BadRequest("Only the owner of workspace can invite others!");
            }
            
            if (workspace.Members.Any(member => member.UserId == inviteDto.InviteeId))
            {
                return BadRequest("Invitee is already a member of the workspace.");
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                if (userId == inviteDto.InviterId || userTokenRole == "Admin")
                {
                    var inviteModel = _mapper.Map<Invite>(inviteDto);
                    await _inviteRepo.AddInviteAsync(inviteModel);
                    return CreatedAtAction(nameof(GetInviteById), new { id = inviteModel.InviteId },
                        _mapper.Map<InviteDto>(inviteModel));
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("UpdateInvite")]
        public async Task<IActionResult> UpdateInvite(UpdateInviteDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 
            try
            {
                var invite = await _inviteRepo.GetInviteByIdAsync(updateDto.InviteId);
                if (invite == null) return BadRequest("Invite not found!");
                
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var inviterId = invite.InviterId;
                var inviteeId = invite.InviteeId;
                if (userId == inviteeId || userId == inviteeId || userTokenRole == "Admin")
                {

                    var inviteUpdate = await _inviteRepo.UpdateInviteStatusAsync(updateDto);
                    if (inviteUpdate == null) return NotFound("Invite Not Found!");

                    var inviteDto = _mapper.Map<InviteDtoOut>(inviteUpdate);
                    return Ok(inviteDto);
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeleteInviteById")]
        public async Task<IActionResult> DeleteInviteById([FromQuery] InviteIdDto inviteIdDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var invite = await _inviteRepo.GetInviteByIdAsync(inviteIdDto.InviteId);

                if (invite == null) return BadRequest("Invite not found!");
                
                var inviterId = invite.InviterId;
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                if (userId == inviterId || userTokenRole == "Admin")
                {

                    var inviteModel = await _inviteRepo.DeleteInviteAsync(inviteIdDto.InviteId);
                    if (inviteModel == null) return NotFound("Invite Not Found!");

                    return Ok("Invite Deleted!");
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error! " + e.Message);
            }
        }


    }
}

















