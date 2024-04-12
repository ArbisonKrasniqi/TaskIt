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

        public BoardController(IBoardRepository boardRepo)
        {
            _boardRepo = boardRepo;
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
        public async Task<IActionResult> Create([FromRoute] int workspaceId, CreateBoardDto boardDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var boardModel = boardDto.ToBoardFromCreate(workspaceId);
            await _boardRepo.CreateAsync(boardModel);
            return CreatedAtAction(nameof(GetById), new {id = boardModel.Id}, boardModel.ToBoardDto());
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

        [HttpDelete(template:"DeleteBoardsWorkpaceID")]
        public async Task<IActionResult> DeleteByWorkspace(int workspaceId)
        {
            var deletedBoards = await _boardRepo.DeleteBoardsAsync(workspaceId);
            
            if (deletedBoards.Count == 0)
                return NotFound(); 

            return Ok(deletedBoards);
        }
    }
}