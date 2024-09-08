using AutoMapper;
using backend.DTOs.Board.Input;
using backend.DTOs.Board.Output;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMembersRepository _membersRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IWorkspaceActivityRepository _workspaceActivityRepo;

        public BoardController(IBoardRepository boardRepo,IWorkspaceRepository workspaceRepo, IBackgroundRepository backgroundRepo, IMembersRepository membersRepo, IUserRepository userRepo, IMapper mapper, IWorkspaceActivityRepository workspaceActivityRepo)
        {
            _boardRepo = boardRepo;
            _workspaceRepo = workspaceRepo;
            _backgroundRepo = backgroundRepo;
            _membersRepo = membersRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _workspaceActivityRepo = workspaceActivityRepo;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetBoardsByWorkspaceId")]
        public async Task<IActionResult> GetBoardsByWorkspaceId(int workspaceId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                if (!await _workspaceRepo.WorkspaceExists(workspaceId))
                {
                    return NotFound("Workspace Not Found!");
                }

                if (userTokenRole == "Admin")
                {
                    var boards = await _boardRepo.GetBoardsByWorkspaceIdAsync(workspaceId);
                    
                    if (boards?.Count == 0)
                    {
                        return Ok(new List<BoardDto>()); // Return an empty list if no boards are found
                    }

                    var boardDtos = _mapper.Map<IEnumerable<BoardDto>>(boards);
                    return Ok(boardDtos);
                }
                var isMember = await _membersRepo.IsAMember(userId, workspaceId);
                if (isMember)
                {
                    var unclosedBoards = await _boardRepo.GetUnclosedBoardsAsync(workspaceId);
                    if (unclosedBoards.Count == 0)
                    {
                        return Ok(new List<BoardDto>());
                    }
                    
                    var unClosedBoardsDto = _mapper.Map<IEnumerable<BoardDto>>(unclosedBoards);
                    return Ok(unClosedBoardsDto);
                    
                }

                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetBoardByID")]
        public async Task<IActionResult> GetBoardById(int id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var board = await _boardRepo.GetBoardByIdAsync(id);

                if (board == null)
                    return NotFound("Board Not Found!");
                
                var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
                if (workspace == null)
                {
                    return NotFound("Workspace not found");
                }

                var isOwner = await _userRepo.UserOwnsWorkspace(userId, board.WorkspaceId);
                if (isOwner || userTokenRole == "Admin")
                {
                    var boardDto = _mapper.Map<BoardDto>(board);
                    return Ok(boardDto);
                }
                var isMember = await _membersRepo.IsAMember(userId, board.WorkspaceId);

                if (isMember)
                {
                    if (board.IsClosed)
                    {
                        return StatusCode(401, "You are not authorized");
                    }
                    var boardDto = _mapper.Map<BoardDto>(board);
                    return Ok(boardDto);
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("CreateBoard")]
        public async Task<IActionResult> CreateBoard(CreateBoardDto boardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _workspaceRepo.WorkspaceExists(boardDto.WorkspaceId))
            {
                return NotFound("Workspace not Found!");
            }
            if (!await _backgroundRepo.BackgroundExists(boardDto.BackgroundId))
            {
                return NotFound("Background Not Found!");
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                if (userId == null)
                {
                    return NotFound("User not found");
                }
                var isMember = await _membersRepo.IsAMember(userId, boardDto.WorkspaceId);
                if (isMember || userTokenRole == "Admin")
                {
                    var boardModel = _mapper.Map<Board>(boardDto);
                    var createdBoard =  await _boardRepo.CreateBoardAsync(boardModel);
                    var createdBoardDto = _mapper.Map<BoardDto>(createdBoard);
                    
                    var workspaceActivity = new WorkspaceActivity
                    {
                        WorkspaceId = createdBoard.WorkspaceId,
                        UserId = userId,
                        ActionType = "Created",
                        EntityName = "board "+createdBoard.Title,
                        ActionDate = DateTime.Now
                    };
                    
                    await _workspaceActivityRepo.CreateWorkspaceActivityAsync(workspaceActivity);
                    return CreatedAtAction(nameof(GetBoardById), new { id = createdBoard.BoardId }, createdBoardDto);

                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                // Log the inner exception details for better debugging
                var innerExceptionMessage = e.InnerException != null ? e.InnerException.Message : string.Empty;
                return StatusCode(500, $"Internal Server Error! {e.Message} {innerExceptionMessage}");
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("UpdateBoard")]
        public async Task<IActionResult> UpdateBoard(UpdateBoardRequestDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                var board = await _boardRepo.GetBoardByIdAsync(updateDto.BoardId);

                if (board == null)
                    return NotFound("Board Not Found!");

                var backgroundExists = await _backgroundRepo.BackgroundExists(updateDto.BackgroundId);
                if (!backgroundExists)
                {
                    return NotFound("Background does not exist");
                }
                
                var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(board.WorkspaceId);
                if (workspace == null)
                {
                    return NotFound("Old workspace not found");
                }
                
                var isMember = await _membersRepo.IsAMember(userId, workspace.WorkspaceId);
                var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspace.WorkspaceId);
                if (updateDto.IsClosed != null)
                {
                    if (userTokenRole != "Admin" && !isOwner)
                    {
                        return StatusCode(401, "You are not authorized");
                    }
                    
                }
                
                if (board.IsClosed != updateDto.IsClosed && (!isOwner && userTokenRole != "Admin"))
                {
                    if (updateDto.IsClosed != null)
                    {
                        return StatusCode(401, "You are not authorized");
                    }
                }
                if (isMember || userTokenRole == "Admin")
                {
                    var boardModel = await _boardRepo.UpdateBoardAsync(updateDto);

                    if (boardModel == null)
                        return NotFound("Board Not Found!");
                    
                    var workspaceActivity = new WorkspaceActivity
                    {
                        WorkspaceId = board.WorkspaceId,
                        UserId = userId,
                        ActionType = "Updated",
                        EntityName = "board "+board.Title,
                        ActionDate = DateTime.Now
                    };
                    
                    await _workspaceActivityRepo.CreateWorkspaceActivityAsync(workspaceActivity);

                    
                    
                    var boardDto = _mapper.Map<BoardDto>(boardModel);
                    return Ok(boardDto);
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeleteBoardByID")]
        public async Task<IActionResult> DeleteBoard(BoardIdDto boardIdDto)
        {
            if  (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                
                var board = await _boardRepo.GetBoardByIdAsync(boardIdDto.BoardId);

                if (board == null) return NotFound("Board does not exist!");

                var workspaceId = board.WorkspaceId;    
                
                //vetem owner mundet me delete board
                var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, workspaceId);

            
                if (ownsWorkspace || userTokenRole == "Admin")
                {
                        
                    var workspaceActivity = new WorkspaceActivity
                    {
                        WorkspaceId = board.WorkspaceId,
                        UserId = userId,
                        ActionType = "Deleted",
                        EntityName = "board "+board.Title,
                        ActionDate = DateTime.Now
                    };
                    
                    await _workspaceActivityRepo.CreateWorkspaceActivityAsync(workspaceActivity);

                    var boardModel = await _boardRepo.DeleteBoardAsync(boardIdDto.BoardId);
                    if (boardModel == null)
                    {
                        return NotFound("Board not found!");
                    }

                    return Ok("Board Deleted!");
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }
        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete(template:"DeleteBoardsByWorkpaceID")]
        public async Task<IActionResult> DeleteByWorkspace(WorkspaceIdDto workspaceIdDto)
        {
            if  (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                if (!await _workspaceRepo.WorkspaceExists(workspaceIdDto.WorkspaceId))
                {
                    return NotFound("Workspace Not Found!");
                }
                
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                
                var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, workspaceIdDto.WorkspaceId);
                //vetem owner mundet me delete board
                if (ownsWorkspace || userTokenRole == "Admin")
                {
                    var boardModel = await _boardRepo.DeleteBoardsByWorkspaceIdAsync(workspaceIdDto.WorkspaceId);

                    if (boardModel.Count == 0)
                    {
                        return NotFound("Boards Not Found!");
                    }

                    return Ok("Boards Deleted!");
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Close")]
        public async Task<IActionResult> CloseBoard(CloseBoardRequestDto dto)
        {
            if  (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var board = await _boardRepo.GetBoardByIdAsync(dto.BoardId);

                if (board == null) return NotFound("Board does not exists!");

                var workspaceId = board.WorkspaceId;

                //vetem owner mundet me bo close
                var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
                if (ownsWorkspace || userTokenRole == "Admin")
                {
                    var result = await _boardRepo.CloseBoardAsync(dto.BoardId, userId);
                    if (!result)
                    {
                        return NotFound();
                    }

                    return Ok("Board closed");
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Reopen")]
        public async Task<IActionResult> ReopenBoard(CloseBoardRequestDto dto)
        {
            if  (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                var board = await _boardRepo.GetBoardByIdAsync(dto.BoardId);

                if (board == null) return NotFound("Board does not exists!");

                var workspaceId = board.WorkspaceId;

                //vetem owner mundet me reopen
                var ownsWorkspace = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
                if (ownsWorkspace || userTokenRole == "Admin")
                {
                    var result = await _boardRepo.ReopenBoardAsync(dto.BoardId, userId);
                    if (!result)
                    {
                        return NotFound();
                    }

                    return Ok("Board reopened");
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!"+e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetClosedBoards")]
        public async Task<IActionResult> GetClosedBoards(int workspaceId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                var isMember = await _membersRepo.IsAMember(userId, workspaceId);

                if (isMember || userTokenRole == "Admin")
                {
                    var closedBoards = await _boardRepo.GetClosedBoardsAsync(workspaceId);

                    if (closedBoards.Count == 0)
                    {
                        return Ok(new List<BoardDto>());
                    }


                    var closedBoardsDto = _mapper.Map<IEnumerable<BoardDto>>(closedBoards);

                    return Ok(closedBoardsDto);
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetUnclosedBoards")]
        public async Task<IActionResult> GetUnClosedBoards(int workspaceId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
                
                var isMember = await _membersRepo.IsAMember(userId, workspaceId);

                if (isMember || userTokenRole == "Admin")
                {
                    var unclosedBoards = await _boardRepo.GetUnclosedBoardsAsync(workspaceId);

                    if (unclosedBoards.Count == 0)
                    {
                        return Ok(new List<BoardDto>());
                    }


                    var closedBoardsDto = _mapper.Map<IEnumerable<BoardDto>>(unclosedBoards);

                    return Ok(closedBoardsDto);
                }
                return StatusCode(401, "You are not authorized!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }

    }
}