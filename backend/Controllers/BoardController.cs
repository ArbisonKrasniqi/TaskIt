using AutoMapper;
using backend.DTOs.Board.Input;
using backend.DTOs.Board.Output;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("backend/board")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository _boardRepo;
        private readonly IWorkspaceRepository _workspaceRepo;
        private readonly IBackgroundRepository _backgroundRepo;
        private readonly IMapper _mapper;

        public BoardController(IBoardRepository boardRepo,IWorkspaceRepository workspaceRepo, IBackgroundRepository backgroundRepo, IMapper mapper)
        {
            _boardRepo = boardRepo;
            _workspaceRepo = workspaceRepo;
            _backgroundRepo = backgroundRepo;
            _mapper = mapper;
        }

        [HttpGet(template:"GetAllBoards")]
        public async Task<IActionResult> GetAllBoards()
        {
            try
            {
                var boards = await _boardRepo.GetAllBoardsAsync();

                if (boards.Count() == 0)
                    return NotFound("There are no boards");

                var boardDto = _mapper.Map<IEnumerable<BoardDto>>(boards);

                return Ok(boardDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpGet("GetBoardsByWorkspaceId")]
        public async Task<IActionResult> GetBoardsByWorkspaceId(int workspaceId)
        {
            try
            {
                if (!await _workspaceRepo.WorkspaceExists(workspaceId))
                {
                    return NotFound("Workspace Not Found!");
                }
                
                var boards = await _boardRepo.GetBoardsByWorkspaceIdAsync(workspaceId);
                
                if (boards.Count == 0)
                {
                    return  Ok(new List<BoardDto>()); // Return an empty list if no boards are found
                }

                var boardDtos = _mapper.Map<IEnumerable<BoardDto>>(boards);
                return Ok(boardDtos);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpGet("GetBoardByID")]
        public async Task<IActionResult> GetBoardById(int id)
        {
            try
            {
                var board = await _boardRepo.GetBoardByIdAsync(id);

                if (board == null)
                    return NotFound("Board Not Found!");

                var boardDto = _mapper.Map<BoardDto>(board);
                return Ok(boardDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpPost("CreateBoard")]
        public async Task<IActionResult> CreateBoard(CreateBoardDto boardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _workspaceRepo.WorkspaceExists(boardDto.WorkspaceId))
            {
                return BadRequest("Workspace not Found!");
            }
            if (!await _backgroundRepo.BackgroundExists(boardDto.BackgroundId))
            {
                return BadRequest("Background Not Found!");
            }

            try
            {
                var boardModel = _mapper.Map<Board>(boardDto);
                await _boardRepo.CreateBoardAsync(boardModel);
                return CreatedAtAction(nameof(GetBoardById), new { id = boardModel.BoardId }, _mapper.Map<BoardDto>(boardModel));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpPut]
        [Route("UpdateBoard")]
        public async Task<IActionResult> UpdateBoard(UpdateBoardRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var board = await _boardRepo.UpdateBoardAsync(updateDto);
    
                if (board == null)
                    return NotFound("Board Not Found!");

                var boardDto = _mapper.Map<BoardDto>(board);
                return Ok(boardDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteBoardByID")]
        public async Task<IActionResult> DeleteBoard([FromQuery] BoardIdDto boardIdDto)
        {
            if  (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var boardModel = await _boardRepo.DeleteBoardAsync(boardIdDto.BoardId);

                if (boardModel == null)
                    return NotFound("Board Does Not Exist!");

                return Ok("Board Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpDelete(template:"DeleteBoardsByWorkpaceID")]
        public async Task<IActionResult> DeleteByWorkspace(WorkspaceIdDto workspaceIdDto)
        {
            if (!await _workspaceRepo.WorkspaceExists(workspaceIdDto.WorkspaceId))
            {
                return StatusCode(404, "Workspace Not Found!");
            }
            
            if  (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var boardModel = await _boardRepo.DeleteBoardsByWorkspaceIdAsync(workspaceIdDto.WorkspaceId);

                if (boardModel.Count == 0)
                {
                    return NotFound("Boards Not Founded!");
                }

                return Ok("Boards Deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpPost("Close")]
        public async Task<IActionResult> CloseBoard(CloseBoardRequestDto dto)
        {
            try
            {
                var result = await _boardRepo.CloseBoardAsync(dto.BoardId, dto.userId);
                if (!result)
                {
                    return NotFound();
                }

                return Ok("Board closed");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpPost("Reopen")]
        public async Task<IActionResult> ReopenBoard(CloseBoardRequestDto dto)
        {
            try
            {
                var result = await _boardRepo.ReopenBoardAsync(dto.BoardId, dto.userId);
                if (!result)
                {
                    return NotFound();
                }

                return Ok("Board reopened");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }

        [HttpGet("GetClosedBoards")]
        public async Task<IActionResult> GetClosedBoards(int workspaceId)
        {
            try
            {
                var closedBoards = await _boardRepo.GetClosedBoardsAsync(workspaceId);

                if (closedBoards.Count == 0)
                {
                    return  Ok(new List<BoardDto>()); 
                }
                
                
                var closedBoardsDto = _mapper.Map<IEnumerable<BoardDto>>(closedBoards);

                return Ok(closedBoardsDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }


    }
}