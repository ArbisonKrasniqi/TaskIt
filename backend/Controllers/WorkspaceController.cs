using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;


namespace backend.Controllers
{
    [Route("backend/workspace")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceRepository _workspaceRepo;
        private readonly IUserRepository _userRepo; 
        private readonly IMapper _mapper;
        private readonly IWorkspaceActivityRepository _workspaceActivityRepo;

        private readonly IBoardActivityRepository _boardActivityRepo;
        public WorkspaceController(IWorkspaceRepository workspaceRepo, IUserRepository userRepo, IMapper mapper, IWorkspaceActivityRepository workspaceActivityRepo, IBoardActivityRepository boardActivityRepo)
        {
            _userRepo = userRepo;
            _workspaceRepo = workspaceRepo;
            _mapper = mapper;
            _workspaceActivityRepo = workspaceActivityRepo;
            _boardActivityRepo = boardActivityRepo;
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetWorkspacesByOwnerId(string ownerId)
        {
            try
            { 
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                if (userId == ownerId || userTokenRole == "Admin")
                {
                    var workspaces = await _workspaceRepo.GetWorkspacesByOwnerIdAsync(ownerId);
                    if (workspaces.Count == 0)
                    {
                        return Ok(new List<WorkspaceDto>());
                    }

                    var workspaceDtos = _mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
                    return Ok(workspaceDtos);
                }

                return StatusCode(401, "You are not authorized!");

            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error!"+ e.Message);
            }
        }

        [HttpGet("GetWorkspacesByMemberId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetWorkspacesByMemberId(string memberId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                if (userId == memberId || userTokenRole == "Admin")
                {
                    var workspaces = await _workspaceRepo.GetWorkspacesByMemberIdAsync(memberId);
                    if (workspaces.Count == 0)
                    {
                        return Ok(new List<WorkspaceDto>());
                    }

                    var workspaceDtos = _mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
                    return Ok(workspaceDtos);
                }

                return StatusCode(401, "You are not authorized!");
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateWorkspace(CreateWorkspaceRequestDto workspaceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!await _userRepo.UserExists(workspaceDto.OwnerId))
                {
                    return NotFound("User not found");
                }
                
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                if (userId == null)
                {
                    return NotFound("User not found");
                }
                
                if (userId == workspaceDto.OwnerId || userTokenRole == "Admin")
                {
                    var workspaceModel = _mapper.Map<Workspace>(workspaceDto);
                    var createdWorkspace = await _workspaceRepo.CreateWorkspaceAsync(workspaceModel);
                    var createdWorkspaceDto = _mapper.Map<WorkspaceDto>(createdWorkspace);

                    var workspaceActivity = new WorkspaceActivity
                    {
                        WorkspaceId = createdWorkspace.WorkspaceId,
                        UserId = userId,
                        ActionType = "Created",
                        EntityName = "workspace "+workspaceDto.Title,
                        ActionDate = DateTime.Now
                    };
                    
                    await _workspaceActivityRepo.CreateWorkspaceActivityAsync(workspaceActivity);


                    //Created BoardActivity
                    var boardActivity = new BoardActivity{
                        BoardId = createdWorkspace.WorkspaceId,
                        UserId = userId,
                        ActionType = "created",
                        EntityName = "workspace " + workspaceDto.Title,
                        ActionDate = DateTime.Now
                    };
                    await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);


                    return CreatedAtAction(nameof(GetWorkspaceById), new { id = createdWorkspace.WorkspaceId }, createdWorkspaceDto); 
                    //kthe pergjigjie 201 Created per burimin e ri te krijuar 
                }
                return StatusCode(401, "You are not authorized!");
                
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error: " + e.Message);
            }
        }

        [HttpPut("UpdateWorkspace")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateWorkspace(UpdateWorkspaceRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                var isOwner = await _userRepo.UserOwnsWorkspace(userId, updateDto.WorkspaceId);
                var newOwnerIsMember = await _userRepo.UserIsMember(userId, updateDto.WorkspaceId);
                
                if (!newOwnerIsMember && (userTokenRole != "Admin" || !isOwner))
                {
                    return StatusCode(401, "You are not authorized");
                }
                if (isOwner || userTokenRole == "Admin")
                {
                    var workspaceModel = await _workspaceRepo.UpdateWorkspaceAsync(updateDto);
                    if (workspaceModel == null)
                    {
                        return NotFound("Workspace not found!");
                    }

                    var workspaceActivity = new WorkspaceActivity
                    {
                        WorkspaceId = updateDto.WorkspaceId,
                        UserId = userId,
                        ActionType = "Updated",
                        EntityName = "workspace "+updateDto.Title,
                        ActionDate = DateTime.Now
                    };
                    
                    await _workspaceActivityRepo.CreateWorkspaceActivityAsync(workspaceActivity);

                    //Updated BoardActivity
                    var boardActivity = new BoardActivity{
                        BoardId = updateDto.WorkspaceId,
                        UserId = userId,
                        ActionType = "updated",
                        EntityName = "workspace " + updateDto.Title,
                        ActionDate = DateTime.Now
                    };
                    await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);

                    
                    var updatedWorkspaceDto = _mapper.Map<WorkspaceDto>(workspaceModel);
                    return Ok(updatedWorkspaceDto);
                }
                return StatusCode(401, "You are not authorized!");
                
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error"+e.Message);
            }
        }

        [HttpDelete("DeleteWorkspace")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteWorkspace(WorkspaceIdDto workspaceIdDto)
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
            
                if (await _userRepo.UserOwnsWorkspace(userId, workspaceIdDto.WorkspaceId) || userTokenRole == "Admin") //If user owns workspace or is admin
                {
                    var workspaceModel = await _workspaceRepo.DeleteWorkspaceAsync(workspaceIdDto.WorkspaceId);
                    if (workspaceModel == null)
                    {
                        return NotFound("Workspace not found!");
                    }

                    return Ok("Workspace Deleted!");  
                }

                return StatusCode(401, "You are not authorized!");

            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error"+e.Message);
            }
        }

        [HttpDelete("deleteWorkspacesByOwnerId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteWorkspacesByOwnerId(OwnerIdDTO ownerIdDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var ownerExists = await _userRepo.UserExists(ownerIdDto.OwnerId);
                if (!ownerExists)
                {
                    return NotFound("Owner not found");
                }
                if (userId == ownerIdDto.OwnerId || userTokenRole == "Admin")
                {
                    var workspaceModels = await _workspaceRepo.DeleteWorkspacesByOwnerIdAsync(ownerIdDto.OwnerId);
                    if (workspaceModels.Count==0)
                    {
                        return NotFound("Workspaces not found!");
                    }

                    return Ok("Workspaces Deleted!");
                }

                return StatusCode(401, "You are not authorized!");

            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error!"+e.Message);
            }
        }
     

    }
}