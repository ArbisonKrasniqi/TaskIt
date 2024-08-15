using AutoMapper;
using backend.DTOs.StarredBoard;

using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers

{
[Route("backend/starredBoard")]
[ApiController]

    public class StarredBoardController : ControllerBase
    {

        private readonly IStarredBoardRepository _starredBoardRepo;
        private readonly IMembersRepository _membersRepo;
        private readonly IMapper _mapper;


        public StarredBoardController(IStarredBoardRepository starredBoardRepo, IMembersRepository membersRepo, IMapper mapper)
        {
            _starredBoardRepo = starredBoardRepo;
            _membersRepo = membersRepo;
            _mapper = mapper;
        }

        //CREATE
        //STAR BOARD
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("StarBoard")]
        public async Task<IActionResult> StarBoard(StarBoardRequestDto starredBoardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var isMember = await _membersRepo.IsAMember(starredBoardDto.UserId, starredBoardDto.WorkspaceId);
                
                if (isMember || userId == starredBoardDto.UserId || userTokenRole == "Admin")
                {
                    var starredModel = _mapper.Map<StarredBoard>(starredBoardDto);

                    var starredBoard =
                        await _starredBoardRepo.StarBoardAsync(starredModel.UserId, starredModel.BoardId);

                    if (starredBoard == null)
                    {
                        return NotFound(new { message = "Could not star this board!" });
                    }

                    var starredBoardDtoNew = _mapper.Map<StarredBoardDto>(starredBoard);
                    return CreatedAtAction(nameof(GetStarredBoardById), new { id = starredBoard.StarredBoardId },
                        starredBoardDtoNew);
                }

                return StatusCode(401, "You are not authorized!");
            }
            catch(Exception e)
            {
                return StatusCode(500, "Internal Server Error: " + e.Message);
            }
        }
     


        
        
        //GET ALL 
        //GET STARRED BOARDS BY USER
        [HttpGet("GetStarredBoardsByUserId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetStarredBoardsByUserId(string userId)
        {
            try
            {

                var userID = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                if (userID == userId || userTokenRole == "Admin")
                {
                    var starredBoards = await _starredBoardRepo.GetStarredBoardsAsync(userId);
                    if (starredBoards.Count == 0)
                    {
                        return Ok(new List<StarredBoardDto>());
                    }

                    var starredBoardsDtos = _mapper.Map<IEnumerable<StarredBoardIDDto>>(starredBoards);
                    return Ok(starredBoardsDtos);
                }

                return StatusCode(401, "You are not authorized!");
            }catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error! " + e.Message);
            }
        }
        
        //GET
        //GET STARRED BOARDS BY WORKSPACE
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetStarredBoardsByWorkspaceId")]
        public async Task<IActionResult> GetStarredBoardsByWorkspaceId(int workspaceId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var isMember = await _membersRepo.IsAMember(userId, workspaceId );
                if (isMember || userTokenRole == "Admin")
                {
                    var starredBoards = await _starredBoardRepo.GetStarredBoardsByWorkspaceAsync(userId, workspaceId);
                    if (starredBoards.Count == 0)
                    {
                        return Ok(new List<StarredBoardDto>());
                    }
                    var starredBoardsDtos = _mapper.Map<IEnumerable<StarredBoardIDDto>>(starredBoards);
                    return Ok(starredBoardsDtos);
                }
                return StatusCode(401, "You are not authorized!");
                }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error! " + e.Message);
            }
        }

        //GET
        //GET STARRED BOARD BY ID
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetStarredBoardById{id}")] //mekthy bordin e bere star ne baze te id se tij
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UnStarBoard([FromQuery] UnStarBoardRequestDto unStarBoardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var isMember = await _membersRepo.IsAMember(unStarBoardDto.UserId, unStarBoardDto.WorkspaceId);
                if (isMember || userId == unStarBoardDto.UserId || userTokenRole == "Admin")
                {
                    var unstarredBoardModel = _mapper.Map<StarredBoard>(unStarBoardDto);

                    var unstarredBoard =
                        await _starredBoardRepo.UnStarBoardAsync(unstarredBoardModel.UserId,
                            unstarredBoardModel.BoardId);
                    if (unstarredBoard == null)
                    {
                        return NotFound("Board Not Found or Not Starred!");
                    }

                    var unstarredBoardDto = _mapper.Map<StarredBoardDto>(unstarredBoard);

                    return Ok(unstarredBoardDto);
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error "+ e.Message);
            }
        }

    }
}