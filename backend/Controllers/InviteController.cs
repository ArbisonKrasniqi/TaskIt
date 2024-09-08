using AutoMapper;
using backend.DTOs.Invite.Input;
using backend.DTOs.Invite.Output;
using backend.DTOs.Members;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IMembersRepository _memberRepo;
        private readonly IMapper _mapper;
        private readonly IWorkspaceActivityRepository _workspaceActivityRepo;
        private readonly UserManager<User> _userManager;

        public InviteController(IInviteRepository inviteRepo, IWorkspaceRepository workspaceRepo, IUserRepository userRepo,IMembersRepository memberRepo, IMapper mapper, IWorkspaceActivityRepository workspaceActivityRepo, UserManager<User> userManager)
        {
            _inviteRepo = inviteRepo;
            _workspaceRepo = workspaceRepo;
            _userRepo = userRepo;
            _memberRepo = memberRepo;
            _mapper = mapper;
            _workspaceActivityRepo = workspaceActivityRepo;
            _userManager = userManager;
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
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User Not Found!");
                }
                var isMember = await _memberRepo.IsAMember(userId, workspaceId);
                if (isMember || userTokenRole == "Admin")
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
            if (workspace == null)
            {
                return NotFound("Workspace Not Found!");
            }

            if (workspace.OwnerId == inviteDto.InviteeId)
            {
                return BadRequest("You can not invite the owner of the workspace");
            }
            
            if (workspace.Members.Any(member => member.UserId == inviteDto.InviteeId))
            {
                return BadRequest("Invitee is already a member of the workspace.");
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User Not Found!");
                }
                var isMember = await _memberRepo.IsAMember(userId, inviteDto.WorkspaceId);
                if (isMember && userId == inviteDto.InviterId || userTokenRole == "Admin")
                {
                    
                    var inviteModel = _mapper.Map<Invite>(inviteDto);
                    await _inviteRepo.AddInviteAsync(inviteModel);

                    var invitee = await _userManager.FindByIdAsync(inviteDto.InviteeId);
                    if (invitee == null)
                    {
                        return NotFound("Invitee Not Found!");
                    }

                    var workspaceActivity = new WorkspaceActivity
                    {
                        WorkspaceId = inviteDto.WorkspaceId,
                        UserId = userId,
                        ActionType = "Invited",
                        EntityName = " "+invitee.Email+" to join workspace "+workspace.Title,
                        ActionDate = DateTime.Now
                    };
                    await _workspaceActivityRepo.CreateWorkspaceActivityAsync(workspaceActivity);
                    
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
        [HttpPut("UpdateInviteStatus")]
        public async Task<IActionResult> UpdateInviteStatus(UpdateInviteDto updateDto)
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
                var inviteeId = invite.InviteeId;
                if (userId == inviteeId || userTokenRole == "Admin")
                {

                    var inviteUpdate = await _inviteRepo.UpdateInviteStatusAsync(updateDto);
                    if (inviteUpdate == null) return NotFound("Invite Not Found!");

                    var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(invite.WorkspaceId);
                    if (workspace == null)
                    {
                        return NotFound("Workspace Not Found!");
                    }
                    
                    if (updateDto.InviteStatus == "Accepted")
                    {
                        
                        var workspaceActivity = new WorkspaceActivity
                        {
                            WorkspaceId = invite.WorkspaceId,
                            UserId = inviteeId,
                            ActionType = "Joined",
                            EntityName = "workspace "+workspace.Title,
                            ActionDate = DateTime.Now
                        };
                        await _workspaceActivityRepo.CreateWorkspaceActivityAsync(workspaceActivity);
                    }
                   
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
                
             
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User Not Found!");
                }
                var isOwner = await _userRepo.UserOwnsWorkspace(userId, invite.WorkspaceId);
                if (isOwner || userTokenRole == "Admin")
                {

                    var inviteModel = await _inviteRepo.DeleteInviteAsync(inviteIdDto.InviteId);
                    if (inviteModel)
                    {
                        return Ok("Invite Deleted!");
                    }
                        
                        return NotFound("Invite Not Found!");
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error! " + e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpPut(template:"UpdateInvite")]
        public async Task<IActionResult> UpdateInvite(UpdateInviteAdminDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var invite = await _inviteRepo.GetInviteByIdAsync(updateDto.InviteId);
                if (invite == null)
                {
                    return NotFound("Invite not found");
                }

                var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(updateDto.WorkspaceId);
                if (workspace == null)
                {
                    return NotFound("Workspace not found");
                }
                
                var inviterIsMember = await _userRepo.UserIsMember(updateDto.InviterId, updateDto.WorkspaceId);
                if (!inviterIsMember)
                {
                    return StatusCode(403, "Inviter does not own workspace");
                }
                var inviteeExists = await _userRepo.UserExists(updateDto.InviteeId);
                if (!inviteeExists)
                {
                    return NotFound("Invitee does not exist");
                }
                
                
                if (invite.InviteStatus != updateDto.InviteStatus)
                {
                    switch (updateDto.InviteStatus)
                    {
                        case "Accepted":
                            var isMemberAccepted = await _memberRepo.IsAMember(updateDto.InviteeId, updateDto.WorkspaceId);
                            if (!isMemberAccepted)
                            {
                                var addMemberDto = new AddMemberDto
                                {
                                    UserId = updateDto.InviteeId,
                                    WorkspaceId = updateDto.WorkspaceId
                                };
                                await _memberRepo.AddMemberAsync(addMemberDto);
                            }
                            break;
        
                        case "Pending":

                            var isMemberPending = await _memberRepo.IsAMember(updateDto.InviteeId, updateDto.WorkspaceId);
                            if (isMemberPending)
                            {
                                await _memberRepo.RemoveMemberAsync(updateDto.WorkspaceId, updateDto.InviteeId);
                            }

                            break;

                        case "Declined":
                            var isMemberDeclined = await _memberRepo.IsAMember(updateDto.InviteeId, updateDto.WorkspaceId);
                            if (isMemberDeclined)
                            {
                                await _memberRepo.RemoveMemberAsync(updateDto.WorkspaceId, updateDto.InviteeId);
                            }
                            break;

                        default:
                            return BadRequest("InviteStatus BadRequest: Only values 'Accepted', 'Pending', and 'Declined' are allowed");
                    }
                }
                
                var inviteResponse = await _inviteRepo.UpdateInviteAsync(updateDto);
                
                if (inviteResponse == null) return NotFound("Invite not found!");

                var inviteDto = _mapper.Map<InviteDto>(inviteResponse);
                return Ok(inviteDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpDelete("DeleteAllInvitesByWorkspaceId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteByWorkspace([FromQuery] WorkspaceIdDto workspaceIdDto)
        {
            if  (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _workspaceRepo.WorkspaceExists(workspaceIdDto.WorkspaceId))
            {
                return StatusCode(404, "Workspace Not Found!");
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User Not Found!");
                }
                var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, workspaceIdDto.WorkspaceId);
                if (ownsWorkspace || userTokenRole == "Admin")
                {
                    var invitesModel = await _inviteRepo.DeleteInvitesByWorkspaceIdAsync(workspaceIdDto.WorkspaceId);
                    if (invitesModel.Count == 0) return NotFound("Invites not found!");

                    return Ok("Invites deleted!");
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpGet("Check-pending-invite")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CheckPendingInviteExists([FromQuery]checkInviteDto checkInviteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User Not Found!");
                }
                var isMember = await _memberRepo.IsAMember(userId, checkInviteDto.WorkspaceId);
                if (isMember && userId == checkInviteDto.InviterId || userTokenRole == "Admin")
                {

                    bool pendingInviteExists = await _inviteRepo.PendingInviteExistsAsync(checkInviteDto.InviterId,
                        checkInviteDto.InviteeId, checkInviteDto.WorkspaceId);
                   
                    if (pendingInviteExists)
                    {
                        return Ok(new { exists = true, message = "Pending invite exists." });
                    }
                    else
                    {
                        return Ok(new { exists = false, message = "No pending invite found." });
                    }
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }
        
    }
}

















