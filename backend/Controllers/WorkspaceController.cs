using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace backend.Controllers;
[Route("backend/workspace")]
//means that the controller or action method will be accessible using
//an HTTP request to http://yourdomain/backend/workspace
[ApiController]

public class WorkspaceController : ControllerBase
{
    private readonly IWorkspaceRepository _workspaceRepo; //qe me kon immutable
    private readonly IUserRepository _userRepo;

    public WorkspaceController(IWorkspaceRepository workspaceRepo, IUserRepository userRepo)
    {
        _userRepo = userRepo;
        _workspaceRepo = workspaceRepo;
    }
    
    //GETALL
    [HttpGet]
    public async Task<IActionResult> GetAllWorkspaces()
    {
        var workspaces = await _workspaceRepo.GetAllWorkspacesAsync(); //list
        var workspaceDto = workspaces.Select(s => s.ToWorkspaceDto());
     //Select osht si mapper dhe e kthen nje immutable array ose list te toWorkspaceDto   
     return Ok(workspaceDto); //serialized to JSON 200 .
    }

    [HttpGet("GetWorkspaceByOwner/{ownerId}")]
    public async Task<IActionResult> GetWorkspacesByOwnerId([FromRoute] string ownerId)
    {
        var workspaces = await _workspaceRepo.GetWorkspacesByOwnerIdAsync(ownerId);
        return Ok(workspaces);
    }

    //GETBYID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspaceById([FromRoute] int id)
    {
        var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(id);
        if (workspace == null)
        {
            return NotFound();
        }

        return Ok(workspace.ToWorkspaceDto());
    }
    
    //CREATE
    [HttpPost("CreateWorkspace")]
    public async Task<IActionResult> CreateWorkspace ([FromBody] CreateWorkspaceRequestDto workspaceDto) //fromBody sepse nuk jena tu i pas te dhanat ne URL po ne body te HTTP
    {
        if (!await _userRepo.UserExists(workspaceDto.OwnerId))
        {
            return BadRequest("User does not exist");
        }
        var workspaceModel = workspaceDto.ToWorkspaceFromCreate(workspaceDto.OwnerId); //e kthen prej DTO ne workspace
        await _workspaceRepo.CreateWorkspaceAsync(workspaceModel);
        return CreatedAtAction(nameof(GetWorkspaceById), new { id = workspaceModel.WorkspaceId}, workspaceModel.ToWorkspaceDto());
        //e ekzekuton metoden getbyId edhe ja qon id e objektit te ri stockModel dhe e kthen ne formen e ToStockDto
    }

    //UPDATE
    [HttpPut]
    [Route("{id}")] //id vjen prej url prandaj e kemi FromRoute
    public async Task<IActionResult> UpdateWorkspace([FromRoute] int id, [FromBody] UpdateWorkspaceRequestDto updateDto)
    {
        var workspaceModel = await _workspaceRepo.UpdateWorkspaceAsync(id, updateDto);
        if (workspaceModel == null)
        {
            return NotFound();
        }

        return Ok(workspaceModel.ToWorkspaceDto());
    }

    //DELETE
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteWorkspace([FromRoute] int id)
    {
        var workspaceModel = await _workspaceRepo.DeleteWorkspaceAsync(id);
        if (workspaceModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    //DELETE ALL WORKSPACES
    [HttpDelete]
    [Route("deleteWorkspacesByOwnerId{ownerId}")]
    public async Task<IActionResult> DeleteWorkspacesByOwnerId([FromRoute] string ownerId)
    {
        if (!await _userRepo.UserExists(ownerId))
        {
            return StatusCode(404, "User not found");
        }
        var workspaceModel = await _workspaceRepo.DeleteWorkspacesByOwnerIdAsync(ownerId);
        if (workspaceModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }




}