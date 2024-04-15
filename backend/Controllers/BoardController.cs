using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("backend/board")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository _boardRepo;
        private readonly IWorkspaceRepository _workspaceRepo;

        public BoardController(IBoardRepository boardRepo,IWorkspaceRepository workspaceRepo)
        {
            _boardRepo = boardRepo;
            _workspaceRepo = workspaceRepo;
        }

        [HttpGet(template:"GetAllBoards")]
        public async Task<IActionResult> GetAllBoards()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var boards = await _boardRepo.GetAllAsync();

            var boardDto = boards.Select(s => s.ToBoardDto());

            return Ok(boardDto);
        }

        [HttpGet("GetBoardsByWorkspaceId/{workspaceId}")]
        public async Task<IActionResult> GetBoardsByWorkspaceId([FromRoute] int workspaceId)
        {
            var boards = await _boardRepo.GetBoardsByWorkspaceIdAsync(workspaceId);
            if (boards == null)
            {
                return BadRequest();
            }

            return Ok(boards);
        }

        [HttpGet("GetBoardByID/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

                var board = await _boardRepo.GetByIdAsync(id);

                if (board == null)
                    return NotFound("Board does not exist");

                return Ok(board.ToBoardDto());
        }

        [HttpPost("CreateBoard/{workspaceId:int}")]
        public async Task<IActionResult> CreateBoard([FromRoute] int workspaceId, [FromBody] CreateBoardDto boardDto)
        {
            if (!await _workspaceRepo.WorkspaceExists(workspaceId))
            {
                return BadRequest("Workspace does not exist");
            }

            var boardModel = boardDto.ToBoardFromCreate(workspaceId);
            await _boardRepo.CreateAsync(boardModel);
            return CreatedAtAction(nameof(GetById), new {id = boardModel.BoardId}, boardModel.ToBoardDto());
        }

        [HttpPut]
        [Route("UpdateBoard/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBoardRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var board = await _boardRepo.UpdateAsync(id, updateDto.ToBoardFromUpdate());

            if (board==null)
                return NotFound("Board not Found");

            return Ok(board.ToBoardDto());
        }

        [HttpDelete]
        [Route("DeleteBoardByID/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if  (!ModelState.IsValid)
                return BadRequest(ModelState);

            var boardModel = await _boardRepo.DeleteAsync(id);

            if (boardModel == null)
                return NotFound("Board does not exist!");
            
            Console.WriteLine("Board has been deleted");
            return Ok(boardModel);
        }

        [HttpDelete(template:"DeleteBoardsByWorkpaceID/{workspaceId}")]
        public async Task<IActionResult> DeleteByWorkspace([FromRoute]int workspaceId)
        {
            if (!await _workspaceRepo.WorkspaceExists(workspaceId))
            {
                return StatusCode(404, "Workspace not found");
            }

            var boardModel = await _boardRepo.DeleteBoardsByWorkspaceIdAsync(workspaceId);

            if (boardModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}