using AutoMapper;
using backend.DTOs.Invite.Input;
using backend.DTOs.Invite.Output;
using backend.Interfaces;
using backend.Models;
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

        [HttpGet("GetInvitesByInviter")]
        public async Task<IActionResult> GetInvitesByInviterId(string inviterId)
        {
            try
            {
                var invites = await _inviteRepo.GetInvitesByInviterAsync(inviterId);
                if (invites.Count() == 0) return Ok(new List<InviteDto>());
              

                var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                return Ok(invitesDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }

        [HttpGet("GetInvitesByInvitee")]
        public async Task<IActionResult> GetInvitesByInviteeId(string inviteeId)
        {
            try
            {
                var invites = await _inviteRepo.GetInvitesByInviteeAsync(inviteeId);
                if (invites.Count() == 0) return Ok(new List<InviteDto>());


                var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                return Ok(invitesDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);

            }
        }

        [HttpGet("GetInvitesByWorkspace")]
        public async Task<IActionResult> GetInvitesByWorkspace(int workspaceId)
        {
            try
            {
                var invites = await _inviteRepo.GetInvitesByWorkspaceAsync(workspaceId);
                if (invites.Count() == 0) return Ok(new List<InviteDto>());
                

                var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                return Ok(invitesDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }

        [HttpGet("GetPendingInvitesByInviter")]
        public async Task<IActionResult> GetPendingInvitesByInviter(string userId)
        {
            try
            {
                var invites = await _inviteRepo.GetPendingInvitesByInviter(userId);
                if (invites.Count() == 0) return Ok(new List<InviteDto>());
                var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                return Ok(invitesDto);
            }  catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }
        [HttpGet("GetPendingInvitesByInvitee")]
        public async Task<IActionResult> GetPendingInvitesByInvitee(string userId)
        {
            try
            {
                var invites = await _inviteRepo.GetPendingInvitesByInvitee(userId);
                if (invites.Count() == 0) return Ok(new List<InviteDto>());
                var invitesDto = _mapper.Map<IEnumerable<InviteDtoOut>>(invites);
                return Ok(invitesDto);
            }  catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }

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

            try
            {
                var inviteModel = _mapper.Map<Invite>(inviteDto);
                await _inviteRepo.AddInviteAsync(inviteModel);
                return CreatedAtAction(nameof(GetInviteById), new { id = inviteModel.InviteId },
                    _mapper.Map<InviteDto>(inviteModel));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }

        }

        [HttpPut]
        [Route("UpdateInvite")]
        public async Task<IActionResult> UpdateInvite(UpdateInviteDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var invite = await _inviteRepo.UpdateInviteStatusAsync(updateDto);
                if (invite == null) return NotFound("Invite Not Found!");

                var inviteDto = _mapper.Map<InviteDtoOut>(invite);
                return Ok(inviteDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteInviteById")]
        public async Task<IActionResult> DeleteInviteById([FromQuery] InviteIdDto inviteIdDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var inviteModel = await _inviteRepo.DeleteInviteAsync(inviteIdDto.InviteId);
                if (inviteModel == null) return NotFound("Invite Not Found!");

                return Ok("Invite Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error! " + e.Message);
            }
        }


    }
}

















