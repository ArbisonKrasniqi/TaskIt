using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.DTOs;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;

namespace backend.Controllers
{
    [Route("backend/workspace")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceRepository _workspaceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public WorkspaceController(IWorkspaceRepository workspaceRepo, IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _workspaceRepo = workspaceRepo;
            _mapper = mapper;
        }

        [HttpGet("GetAllWorkspaces")]
        public async Task<IActionResult> GetAllWorkspaces()
        {
            try
            {
                var workspaces = await _workspaceRepo.GetAllWorkspacesAsync();
                if (workspaces.Count == 0)
                {
                    return Ok(new List<WorkspaceDto>()); //me kthy list te zbrazet
                }

                var workspaceDtos = _mapper.Map<List<WorkspaceDto>>(workspaces);
                return Ok(workspaceDtos);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error!"+e.Message);
            }
        }

        [HttpGet("GetWorkspacesByOwnerId")]
        public async Task<IActionResult> GetWorkspacesByOwnerId(string ownerId)
        {
            try
            {
                var workspaces = await _workspaceRepo.GetWorkspacesByOwnerIdAsync(ownerId);
                if (workspaces.Count == 0)
                {
                    return Ok(new List<WorkspaceDto>());
                }

                var workspaceDtos = _mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
                return Ok(workspaceDtos);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error!"+ e.Message);
            }
        }

        [HttpGet("GetWorkspacesByMemberId")]
        public async Task<IActionResult> GetWorkspacesByMemberId(string memberId)
        {
            try
            {
                var workspaces = await _workspaceRepo.GetWorkspacesByMemberIdAsync(memberId);
                if (workspaces.Count == 0)
                {
                    return Ok(new List<WorkspaceDto>());
                }

                var workspaceDtos = _mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
                return Ok(workspaceDtos);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error!"+ e.Message);
            }
        }

        [HttpGet("GetWorkspaceById")]
        public async Task<IActionResult> GetWorkspaceById(int workspaceId)
        {
            try
            {
                var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(workspaceId);
                if (workspace == null)
                {
                    return NotFound("Workspace not found!");
                }

                var workspaceDto = _mapper.Map<WorkspaceDto>(workspace);
                return Ok(workspaceDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error!"+e.Message);
            }
        }

        [HttpPost("CreateWorkspace")]
        public async Task<IActionResult> CreateWorkspace(CreateWorkspaceRequestDto workspaceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _userRepo.UserExists(workspaceDto.OwnerId))
            {
                return NotFound("User not found");
            }

            try
            {
                var workspaceModel = _mapper.Map<Workspace>(workspaceDto);
                var createdWorkspace = await _workspaceRepo.CreateWorkspaceAsync(workspaceModel);
                var createdWorkspaceDto = _mapper.Map<WorkspaceDto>(createdWorkspace);
                return CreatedAtAction(nameof(GetWorkspaceById), new { id = createdWorkspace.WorkspaceId }, createdWorkspaceDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error: " + e.Message);
            }
        }

        [HttpPut("UpdateWorkspace")]
        public async Task<IActionResult> UpdateWorkspace(UpdateWorkspaceRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var workspaceModel = await _workspaceRepo.UpdateWorkspaceAsync(updateDto);
                if (workspaceModel == null)
                {
                    return NotFound("Workspace not found!");
                }

                var updatedWorkspaceDto = _mapper.Map<WorkspaceDto>(workspaceModel);
                return Ok(updatedWorkspaceDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error"+e.Message);
            }
        }

        [HttpDelete("DeleteWorkspace")]
        public async Task<IActionResult> DeleteWorkspace(WorkspaceIdDto workspaceIdDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var workspaceModel = await _workspaceRepo.DeleteWorkspaceAsync(workspaceIdDto.WorkspaceId);
                if (workspaceModel == null)
                {
                    return NotFound("Workspace not found!");
                }

                return Ok("Workspace Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error"+e.Message);
            }
        }

        [HttpDelete("deleteWorkspacesByOwnerId")]
        public async Task<IActionResult> DeleteWorkspacesByOwnerId(OwnerIdDTO ownerIdDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _userRepo.UserExists(ownerIdDto.OwnerId))
                {
                    return NotFound("User not found!");
                }

                var workspaceModels = await _workspaceRepo.DeleteWorkspacesByOwnerIdAsync(ownerIdDto.OwnerId);
                if (workspaceModels == null)
                {
                    return NotFound("Workspaces not found!");
                }

                return Ok("Workspaces Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error!"+e.Message);
            }
        }
     

    }
}