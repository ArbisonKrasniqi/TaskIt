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
using Microsoft.IdentityModel.Tokens;

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
    [HttpGet("GetAllWorkspaces")]
    public async Task<IActionResult> GetAllWorkspaces()
    {
        try
        {
            var workspaces = await _workspaceRepo.GetAllWorkspacesAsync(); //list
            if (workspaces.Count == 0)
            {
                return NotFound("There are no workspaces!");
            }

            var workspaceDto = workspaces.Select(s => s.ToWorkspaceDto());
            //Select osht si mapper dhe e kthen nje immutable array ose list te toWorkspaceDto   
            return Ok(workspaceDto);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error!");
        }

    }

    [HttpGet("GetWorkspacesByOwnerId")]
    public async Task<IActionResult> GetWorkspacesByOwnerId(string OwnerId)
    {
        try
        {
            var workspaces = await _workspaceRepo.GetWorkspacesByOwnerIdAsync(OwnerId); //per metodat get sbon me qu dto
            if (workspaces.Count == 0)
            {
                return NotFound("Workspaces not found!");
            }

            return Ok(workspaces);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error!");
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
                return NotFound("Workspaces not found!");
            }

            return Ok(workspaces);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error!");
        }
    }
    

    //GETBYID
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

            return Ok(workspace.ToWorkspaceDto());
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error!");
        }
    }

    //CREATE
    [HttpPost("CreateWorkspace")]
    public async Task<IActionResult>CreateWorkspace(CreateWorkspaceRequestDto workspaceDto) //fromBody sepse nuk jena tu i pas te dhanat ne URL po ne body te HTTP
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
            var workspaceModel = workspaceDto.ToWorkspaceFromCreate(); // e kthen prej DTO ne workspace
            var createdWorkspace = await _workspaceRepo.CreateWorkspaceAsync(workspaceModel);
            return CreatedAtAction(nameof(GetWorkspaceById), new { id = createdWorkspace.WorkspaceId }, createdWorkspace.ToWorkspaceDto());
            //e ekzekuton metoden getbyId edhe ja qon id e objektit te ri stockModel dhe e kthen ne formen e ToStockDto
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error"+e.Message);
        }


    }

    //UPDATE
    [HttpPut]
    [Route("UpdateWorkspace")] //id vjen prej url prandaj e kemi FromRoute
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

            return Ok(workspaceModel.ToWorkspaceDto());
        }
    catch(Exception e) 
    {
        return StatusCode(500, "Internal server error");
    }
}

//DELETE
    [HttpDelete]
    [Route("DeleteWorkspace")]
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
            return StatusCode(500, "Internal server error");  
        }
    }

    //DELETE ALL WORKSPACES
    [HttpDelete]
    [Route("deleteWorkspacesByOwnerId")]
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
            return StatusCode(500, "Internal server error!");  
        }
    }
    
    

}