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
    private readonly IBoardRepository _boardRepo;

    public ListController(ApplicationDBContext context, IListRepository listRepo , IBoardRepository boardRepo)
    {
        _context = context;
        _listRepo = listRepo;
        _boardRepo = boardRepo;
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
    
    // Http Post with relationship one to many with board 
    [HttpPost("CreateList/{BoardId}")]

    public async Task<IActionResult> CreateList([FromRoute] int BoardId,[FromRoute] CreateListDTO listDto)
    {
        if (!await _boardRepo.BoardExists(BoardId))
        {
            return BadRequest("Board did not exist");
        }

        var listModel = listDto.ToListFromCreate(BoardId);
        await _listRepo.CreateAsync(listModel);
        return CreatedAtAction(nameof(GetById), new { ListId = listModel.ListId }, listModel.ToListDto());
    }

    [HttpGet("GetListByBoardId")]
    public async Task<IActionResult> GetListByBoardId([FromRoute] int BoardId)
    {
        var list = await _listRepo.GetListByBoardId(BoardId);

        if (list == null)
        {
            return NotFound();
        }

        return Ok(list.ToListDto());
    }
    
}
