using Application.Dtos.ListDtos;
using Application.Services.List;
using Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[ApiController]
[Route("backend/list")]
public class ListController : ControllerBase
{
    private readonly IListService _listService;
    
    public ListController(IListService listService)
    {
        _listService = listService;
    }

    [HttpGet("GetAllLists")]
    [Authorize(Policy = "AdminOnly")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetAllLists()
    {
        try
        {
            var lists = await _listService.GetAllLists();
            return Ok(lists);

        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpGet("GetListById")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetLisById(int listId)
    {
        try
        {
            if (int.IsNegative(listId))
            {
                return BadRequest("List Id can not be negative");
            }

            var list = await _listService.GetListById(listId);
            return Ok(list);

        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
    
    [HttpGet("GetListByBoardId")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetLisByBoardId(int boardId)
    {
        try
        {
            if (int.IsNegative(boardId))
            {
                return BadRequest("Board Id can not be negative");
            }

            var lists = await _listService.GetListByBoardId(boardId);
            return Ok(lists);

        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPost("CreateList")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> CreateList(CreateListDto createListDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = await _listService.CreateList(createListDto);
            return Ok(list);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPut("UpdateList")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UpdateList(UpdateListDto updateListDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = await _listService.UpdateList(updateListDto);
            return Ok(list);

        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpDelete("DeleteList")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteList(ListIdDto listIdDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = await _listService.DeleteList(listIdDto);
            return Ok(list);

        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("DragNDropList")]
    public async Task<IActionResult> DragNDropList(DragNDropListDto dragNDropListDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var list = await _listService.DragNDroplist(dragNDropListDto);

            return Ok(list);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
        
    }
}