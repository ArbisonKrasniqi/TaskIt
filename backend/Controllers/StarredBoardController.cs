using AutoMapper;
using backend.DTOs.StarredBoard;

using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers

{
[Route("backend/starredBoard")]
[ApiController]

    public class StarredBoardController : ControllerBase
    {

        private readonly IStarredBoardRepository _starredBoardRepo;
        private readonly IMapper _mapper;


        public StarredBoardController(IStarredBoardRepository starredBoardRepo, IMapper mapper)
        {
            _starredBoardRepo = starredBoardRepo;
            _mapper = mapper;
        }

        //CREATE
        //STAR BOARD
        [HttpPost("StarBoard")]
        public async Task<IActionResult> StarBoard(StarBoardRequestDto starredBoardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var starredModel = _mapper.Map<StarredBoard>(starredBoardDto);

                var starredBoard = await _starredBoardRepo.StarBoardAsync(starredModel.UserId, starredModel.BoardId);
               
                if (starredBoard == null)
                {
                    return NotFound(new { message = "Could not star this board!" });
                }
                
                var starredBoardDtoNew = _mapper.Map<StarredBoardDto>(starredBoard);
                return CreatedAtAction(nameof(GetStarredBoardById), new { id = starredBoard.StarredBoardId },
                    starredBoardDtoNew);

            }
            catch(Exception e)
            {
                return StatusCode(500, "Internal Server Error: " + e.Message);
            }
        }
     


        
        
        //GET ALL 
        //GET STARRED BOARDS BY USER
        [HttpGet("GetStarredBoardsByUserId")]
        public async Task<IActionResult> GetStarredBoardsByUserId(string userId)
        {
            try
            {
                var starredBoards = await _starredBoardRepo.GetStarredBoardsAsync(userId);
                if (starredBoards.Count == 0)
                {
                    return Ok(new List<StarredBoardDto>());
                }

                var starredBoardsDtos = _mapper.Map<IEnumerable<StarredBoardIDDto>>(starredBoards);
                return Ok(starredBoardsDtos);
            }catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error! " + e.Message);
            }
        }
        
        //GET
        //GET STARRED BOARD BY ID
        [HttpGet("{id}")] //mekthy bordin e bere star ne baze te id se tij
        public async Task<IActionResult> GetStarredBoardById(int id)
        {
            try
            {
                var starredBoard = await _starredBoardRepo.GetStarredBoardByIdAsync(id);
                if (starredBoard == null)
                {
                    return NotFound();
                }

                var starredBoardDto = _mapper.Map<StarredBoardDto>(starredBoard);
                return Ok(starredBoardDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error! " + e.Message);
            }
        }

        //DELETE
        [HttpDelete("UnstarBoard")]
        public async Task<IActionResult> UnStarBoard([FromQuery] UnStarBoardRequestDto unStarBoardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var unstarredBoardModel = _mapper.Map<StarredBoard>(unStarBoardDto);
                
                var unstarredBoard = await _starredBoardRepo.UnStarBoardAsync(unstarredBoardModel.UserId, unstarredBoardModel.BoardId);
                if (unstarredBoard == null)
                {
                    return NotFound("Board Not Found or Not Starred!");
                }

                var unstarredBoardDto = _mapper.Map<StarredBoardDto>(unstarredBoard);
    
                return Ok(unstarredBoardDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error "+ e.Message);
            }
        }

    }
}