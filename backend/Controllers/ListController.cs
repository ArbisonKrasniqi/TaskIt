﻿using backend.Data;
using backend.DTOs.Board.Input;
using backend.DTOs.List;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace backend.Controllers;

[Route("backend/list")]
[ApiController]

public class ListController : ControllerBase
{
    private readonly IListRepository _listRepo;
    private readonly IBoardRepository _boardRepo;

    public ListController(IListRepository listRepo , IBoardRepository boardRepo)
    {
        _listRepo = listRepo;
        _boardRepo = boardRepo;
    }

    [HttpGet("GetAllLists")]
    public async Task<IActionResult> GetAllLists()
    {
        try
        {
            var lists = await _listRepo.GetAllListsAsync();

            if (lists.Count == 0)
            {
                return NotFound("There are no lists");
            }
            var listDto = lists.Select(l => l.ToListDto());
            
            return Ok(listDto);
        }
        catch (Exception e)
        {
            return StatusCode(500,"Internal Server Error!");
        }
    }

    [HttpGet("GetListById")]
    public async Task<IActionResult> GetById(int ListId)
    {
        try{
        var list = await _listRepo.GetListByIdAsync(ListId);
        if (list == null)
        {
            return NotFound("List Not Found");
        }
        return Ok(list.ToListDto());
        }catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
    }
    

    [HttpPut("UpdateList")]
    
    public async Task<IActionResult> UpdateList(UpdateListDTO updateListDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var listModel = await _listRepo.UpdateListAsync(updateListDto);

            if (listModel == null)
            {
                return NotFound("List Not Found");
            }
        
            return Ok(listModel.ToListDto());
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }

    [HttpDelete("DeleteList")]

    public async Task<IActionResult> DeleteList(ListIdDTO listIdDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var listModel = await _listRepo.DeleteListAsync(listIdDto.ListId);
            
            if (listModel == null)
            {
                return NotFound("List does not exist");
            }

            return Ok("List Deleted");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }
    
    // Http Post with relationship one to many with board 
    [HttpPost("CreateList")]

    public async Task<IActionResult> CreateList(CreateListDTO listDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (!await _boardRepo.BoardExists(listDto.BoardId))
            {
                return BadRequest("Board did not exist");
            }

            var listModel = listDto.ToListFromCreate();
            await _listRepo.CreateAsync(listModel);
            return CreatedAtAction(nameof(GetById), new { ListId = listModel.ListId }, listModel.ToListDto());
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }

    [HttpGet("GetListByBoardId")]
    public async Task<IActionResult> GetListByBoardId(int BoardId)
    {
        try
        {
            if (!await _boardRepo.BoardExists(BoardId))
            {
                return NotFound("Board Not Found");
            }
            
            var lists = await _listRepo.GetListByBoardId(BoardId);

            if (lists.Count == 0)
            {
                return BadRequest("Lists Not Found");
            }

            return Ok(lists);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Errror!");
        }
    }

    [HttpDelete("DeleteListByBoardId")]
    public async Task<IActionResult> DeleteListByBoardId(BoardIdDto boardIdDto)
    {
        if (!await _boardRepo.BoardExists(boardIdDto.BoardId))
        {
            return StatusCode(404, "Board Not Found");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var listModel = await _listRepo.DeleteListsByBoardIdAsync(boardIdDto.BoardId);

            if (listModel.Count == 0)
            {
                return NotFound("Lists not found!");
            }

            return Ok("Lists Deleted");
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error!");
        }
    }
    
}