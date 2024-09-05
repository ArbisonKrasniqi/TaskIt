using backend.DTOs.Label;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using backend.DTOs.Task;
using backend.Repositories;
using backend.Models;
namespace backend.Controllers;



[Route("backend/label")]
[ApiController]


public class LabelController : ControllerBase{

    private readonly ILabelRepository _labelRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMembersRepository _memberRepo;
    private readonly IMapper _mapperRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IListRepository _listRepo;
    private readonly IWorkspaceRepository _workspaceRepo;

    public LabelController (ILabelRepository labelRepo, IUserRepository userRepo, IMembersRepository memberRepo, IMapper mapperRepo, IBoardRepository boardRepo, IListRepository listRepo, IWorkspaceRepository workspaceRepo){
       
        _labelRepo = labelRepo;
        _userRepo = userRepo;
        _memberRepo = memberRepo;
        _mapperRepo = mapperRepo;
        _boardRepo = boardRepo;
        _listRepo = listRepo;
        _workspaceRepo = workspaceRepo;
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetAllLabels")]
    public async Task<IActionResult> GetAllLabels(){
        try{
            var labels = await _labelRepo.GetAllLabelAsync();
            if(labels.Count == 0){
                return Ok(new List<LabelDto>());
            }

            var labelDtos = _mapperRepo.Map<List<LabelDto>>(labels);
            return Ok(labelDtos);
        }catch(Exception e){
            return StatusCode(500, "Internal server error!" + e.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetLabelsByBoardId")]
    public async Task<IActionResult> GetLabelsByBoardId(int boardId)
    {
        try
        {
            var board = await _boardRepo.GetBoardByIdAsync(boardId);
            if (board == null)
            {
                return NotFound("Board not found");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _memberRepo.IsAMember(userId, workspace.WorkspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }

            if (isMember || userTokenRole == "Admin")
            {
                var labels = await _labelRepo.GetLabelsByBoardId(boardId);
                if (labels.Count == 0)
                {
                    return Ok(new List<LabelDto>());
                }

                var labelDtos = _mapperRepo.Map<List<LabelDto>>(labels);
                return Ok(labelDtos);
            }

            return StatusCode(401, "You are not authorized");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
    }


    /*[Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateLabel")]
    public async Task<IActionResult> CreateLabel(CreateLabelRequestDTO labelDTO){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{
            var board = await _boardRepo.GetBoardByIdAsync(labelDTO.BoardId);
            if (board == null)
            {
                return NotFound("Board not found");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _memberRepo.IsAMember(userId, workspace.WorkspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }

            
            if(isMember || userTokenRole == "Admin"){
                
                //maps the dto to the label model
                var labelModel = _mapperRepo.Map<Label>(labelDTO);
                var createdLabel = await _labelRepo.CreateLabelAsync(labelModel);
                var createdLabelDto = _mapperRepo.Map<LabelDto>(createdLabel);

                return CreatedAtAction(nameof(GetLabelById), new { id = createdLabel.LabelId}, createdLabelDto);

            }

            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal server error");
        }
    }*/

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetLabelById")]
    public async Task<IActionResult> GetLabelById (int labelId){
        try{

            var label = await _labelRepo.GetLabelByIdAsync(labelId);
            if(label == null){
                return NotFound("Label not found");
            }
            var board = await _boardRepo.GetBoardByIdAsync(label.BoardId);
            if(board == null){
                return NotFound("Board not found");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _memberRepo.IsAMember(userId, workspace.WorkspaceId);

            if(isMember || userTokenRole == "Admin"){
                var labelDto = _mapperRepo.Map<LabelDto>(label);
                return Ok(labelDto);
            }

            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal server error!");
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateLabel")]
    public async Task<IActionResult> UpdateLabel(UpdateLabelRequestDTO updateDto){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try
        {

            var label = await _labelRepo.GetLabelByIdAsync(updateDto.LabelId);
            if (label == null)
            {
                return NotFound("Label not found");
            }

            var board = await _boardRepo.GetBoardByIdAsync(label.BoardId);
            if (board == null)
            {
                return NotFound("Board not found");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }

            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var isMember = await _memberRepo.IsAMember(userId, workspace.WorkspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
            
            if(isMember || userTokenRole == "Admin"){
                var labelModel = await _labelRepo.UpdateLabelAsync(updateDto);
                if(labelModel == null){
                    return NotFound("Label not found!");
                }

                var updateLabelDto = _mapperRepo.Map<LabelDto>(labelModel);
                return Ok(updateLabelDto);

            }
            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal server error");
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("DeleteLabel")]

    public async Task<IActionResult> DeleteLabel (LabelIdDTO labelIdDTO){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try
        {
            var label = await _labelRepo.GetLabelByIdAsync(labelIdDTO.LabelId);
            if (label == null)
            {
                return NotFound("Label not found");
            }

            var board = await _boardRepo.GetBoardByIdAsync(label.BoardId);
            if (board == null)
            {
                return NotFound("Board not found");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found");
            }
            
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }
        
            if(await _userRepo.UserIsMember(userId, workspace.WorkspaceId) || userTokenRole == "Admin"){
                
                var labelModel = await _labelRepo.DeleteLabelAsync(labelIdDTO.LabelId);
                if(labelModel == null){
                    return NotFound("Label not found!");
                }

                return Ok("Label Deleted!");
            }

            return StatusCode(401, "You are not authorized!");

        }catch(Exception e){
            return StatusCode(500, "Internal server error");
        }

    }



}