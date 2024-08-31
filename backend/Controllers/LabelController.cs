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

    public LabelController (ILabelRepository labelRepo, IUserRepository userRepo, IMembersRepository memberRepo, IMapper mapperRepo, IBoardRepository boardRepo, IListRepository listRepo){
       
        _labelRepo = labelRepo;
        _userRepo = userRepo;
        _memberRepo = memberRepo;
        _mapperRepo = mapperRepo;
        _boardRepo = boardRepo;
        _listRepo = listRepo;
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
    [HttpPost("CreateLabel")]

    public async Task<IActionResult> CreateLabel(CreateLabelRequestDTO labelDTO){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{
            
            if(!await _boardRepo.BoardExists(labelDTO.BoardId)){
               
                return NotFound("Board not found!");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var board = await _boardRepo.GetBoardByIdAsync(labelDTO.BoardId);
            var workspaceId = board.WorkspaceId;
            var isMember = await _memberRepo.IsAMember(userId, workspaceId);

            if(isMember || userTokenRole == "Admin"){
                
                //maps the dto to the label model
                var labelModel = _mapperRepo.Map<Label>(labelDTO);
                var createdLabel = await _labelRepo.CreateLabelAsync(labelModel);
                var createdLabelDto = _mapperRepo.Map<LabelDto>(createdLabel);

                return CreatedAtAction(nameof(GetLabelById), new { id = createdLabel.LabelId}, createdLabelDto);

            }

            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(401, "You are not athorized!");
        }

    }

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

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _memberRepo.IsAMember(userId, labelId);

            if(isMember || userTokenRole == "Admin"){
                var labelDto = _mapperRepo.Map<LabelDto>(label);
                return Ok(labelDto);
            }

            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(500, "Internal server error!"+e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateLabel")]

    public async Task<IActionResult> UpdateLabel(UpdateLabelRequestDTO updateDto){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{

            if(!await _boardRepo.BoardExists(updateDto.BoardId)){
                return NotFound("Board not found!");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var isMember = await _memberRepo.IsAMember(userId, updateDto.LabelId);
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
            return StatusCode(500, "Internal server error: " + e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")] 
    [HttpDelete("DeleteLabel")]

    public async Task<IActionResult> DeleteLabel ([FromQuery] LabelIdDTO labelIdDTO){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        try{
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        
            if(await _userRepo.UserIsMember(userId, labelIdDTO.LabelId) || userTokenRole == "Admin"){
                
                var labelModel = await _labelRepo.GetLabelByIdAsync(labelIdDTO.LabelId);
                if(labelModel == null){
                    return NotFound("Label not found!");
                }

                return Ok("Label Deleted!");
                
            }

            return StatusCode(401, "You are not authorized!");

        }catch(Exception e){
            return StatusCode(500, "Internal server error: " + e.Message);
        }

    }



}