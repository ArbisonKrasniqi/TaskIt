using AutoMapper;
using backend.DTOs.Board.Input;
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
        private readonly IBoardRepository _boardRepo;
        private readonly IUserRepository _userRepo;

        public StarredBoardController(IStarredBoardRepository starredBoardRepo, IMembersRepository membersRepo,
            IMapper mapper, IBoardRepository boardRepo, IUserRepository userRepo)
        {
            _starredBoardRepo = starredBoardRepo;
            _membersRepo = membersRepo;
            _mapper = mapper;
            _boardRepo = boardRepo;
            _userRepo = userRepo;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetAllStarredBoards")]
        public async Task<IActionResult> GetAllStarredBoards()
        {
            try
            {
                var starredBoards = await _starredBoardRepo.GetAllStarredBoardsAsync();
                if (starredBoards.Count == 0)
                {
                    return Ok(new List<StarredBoardDto>());
                    
                }

                var starredDtos = _mapper.Map<List<StarredBoardDto>>(starredBoards);
                return Ok(starredDtos);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error!"+e.Message);
            }
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
                var board = await _boardRepo.BoardExists(starredBoardDto.BoardId);
                if (!board)
                {
                    return NotFound("Board not found!");
                }

                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var isMember = await _membersRepo.IsAMember(starredBoardDto.UserId, starredBoardDto.WorkspaceId);

                if (isMember || userId == starredBoardDto.UserId || userTokenRole == "Admin")
                {
                    var starredModel = _mapper.Map<StarredBoard>(starredBoardDto);

                    var starredBoard =
                        await _starredBoardRepo.StarBoardAsync(starredModel.UserId, starredModel.BoardId,
                            starredModel.WorkspaceId);

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
            catch (Exception e)
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

                var idUser = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                if (string.IsNullOrEmpty(idUser))
                {
                    return NotFound("User Not Found!");
                }
                if (idUser == userId || userTokenRole == "Admin")
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
            }
            catch (Exception e)
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
                
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User Not Found!");
                }
                var isMember = await _membersRepo.IsAMember(userId, workspaceId);
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
                
                var board = await _boardRepo.BoardExists(unStarBoardDto.BoardId);
                if (!board)
                {
                    return NotFound("Board not found!");
                }
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var isMember = await _membersRepo.IsAMember(unStarBoardDto.UserId, unStarBoardDto.WorkspaceId);
                if (isMember || userId == unStarBoardDto.UserId || userTokenRole == "Admin")
                {
                    var unstarredBoardModel = _mapper.Map<StarredBoard>(unStarBoardDto);

                    var unstarredBoard =
                        await _starredBoardRepo.UnStarBoardAsync(unstarredBoardModel.UserId,
                            unstarredBoardModel.BoardId, unstarredBoardModel.WorkspaceId);
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
                return StatusCode(500, "Internal Server Error " + e.Message);
            }
        }

        [HttpDelete("DeleteStarredBoardByBoardId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteByBoardId([FromQuery] BoardIdDto boardIdDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _boardRepo.BoardExists(boardIdDto.BoardId))
            {
                return StatusCode(404, "Board Not Found!");
            }

            try
            {
                var board = await _boardRepo.GetBoardByIdAsync(boardIdDto.BoardId);
                if (board == null)
                {
                    return NotFound("Board Not Found!");
                }

                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User Not Found!");
                }
                var workspaceId = board.WorkspaceId;
                var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
                if (ownsWorkspace || userTokenRole == "Admin")
                {
                    var starredboardModel = await _starredBoardRepo.DeleteStarredBoardByBoardIdAsync(boardIdDto.BoardId);
                    if (starredboardModel == null) return NotFound("Starred board not found!");

                    return Ok("Starredboard deleted!");
                }

                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!" + e.Message);
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("DeleteStarredBoardById")]
        public async Task<IActionResult> DeleteStarredBoardById([FromQuery] StarredBoardId starredBoardId)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var starredModel = await _starredBoardRepo.DeleteStarredBoardByIdAsync(starredBoardId.StarredId);
                if (starredModel == null) return NotFound("Starred board not found!");
                return Ok("Starred board deleted!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error"+e.Message);
            }
        }

        [HttpPut("UpdateStarredBoard")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateStarredBoard(UpdateStarredBoardDto updateStarredBoardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var starredmodel = await _starredBoardRepo.UpdateStarredBoardAsync(updateStarredBoardDto);
                if (starredmodel == null)
                {
                    return NotFound("Starred board not found!");
                }

                var updatedStarredBoardDto = _mapper.Map<StarredBoardDto>(starredmodel);
                return Ok(updatedStarredBoardDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error" + e.Message);
            }
        }
    }
}