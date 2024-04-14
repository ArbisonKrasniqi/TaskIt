using backend.Data;
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
    private readonly ApplicationDBContext _context;
    private readonly IListRepository _listRepo;

    public ListController(ApplicationDBContext context, IListRepository listRepo)
    {
        _context = context;
        _listRepo = listRepo;
    }

    [HttpGet("GetAllLists")]
    public async Task<IActionResult> GetAllLists()
    {
        var lists = await _listRepo.GetAllListsAsync();
        var listDto = lists.Select(l => l.ToListDto());
        return Ok(lists);
    }

    [HttpGet("GetListById")]
    public async Task<IActionResult> GetById([FromRoute] int ListId)
    {
        var list = await _listRepo.GetListByIdAsync(ListId);
        if (list == null)
        {
            return NotFound();
        }

        return Ok(list.ToListDto());
    }

    [HttpPost("CreateList")]
    public async Task<IActionResult> CreateList([FromBody] CreateListDTO listDto)
    {
        var listModel = listDto.ToListFromCreate();
        await _listRepo.CreateListAsync(listModel);
        return CreatedAtAction(nameof(GetById), new { ListId = listModel.ListId }, listModel.ToListDto());
    }

    [HttpPut("UpdateList/{ListId}")]
    
    public async Task<IActionResult> UpdateList([FromRoute] int ListId, [FromBody] UpdateListDTO updateListDto)
    {
        var listModel = await _listRepo.UpdateListAsync(ListId,updateListDto);

        if (listModel == null)
        {
            return NotFound();
        }
        
        return Ok(listModel.ToListDto());
    }

    [HttpDelete("DeleteList/{ListId}")]

    public async Task<IActionResult> DeleteList([FromRoute] int ListId)
    {
        var listModel = await _listRepo.DeleteListAsync(ListId);
            
        if (listModel == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    
}
