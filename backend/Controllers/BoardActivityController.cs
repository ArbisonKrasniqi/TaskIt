using System.Net;
using backend.DTOs.Board;
using backend.DTOs.Board.Input;
using backend.DTOs.BoardActivity.Input;
using backend.DTOs.BoardActivity.Output;
using backend.DTOs.Workspace;
using backend.Interfaces;
using backend.Mappers.BoardActivity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace backend.Controllers;


public class BoardActivityController : ControllerBase{
    
    private readonly IBoardActivityRepository _boardActivityRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMembersRepository _memberRepo;
    private readonly IUserRepository _userRepo;

    public BoardActivityController(IBoardActivityRepository boardActivityRepo, IBoardRepository boardRepo, IWorkspaceRepository workspaceRepo, IMembersRepository membersRepo, IUserRepository userRepo){
       
        _boardActivityRepo = boardActivityRepo;
        _boardRepo = boardRepo;
        _workspaceRepo = workspaceRepo;
        _memberRepo = membersRepo;
        _userRepo = userRepo;

    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("CreateBoardActivity")]

    public async  Task<IActionResult> CreateBoardActivity(AddBoardActivityDto boardActivityDto){

        try{
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var board = await _boardRepo.GetBoardByIdAsync(boardActivityDto.BoardId);
            if(board == null){
                return NotFound("Board Not Found!");
            }

            var workspaceId = board.WorkspaceId;
            if(userId == null){
                return NotFound("User Not Found!");
            }

            var isMember = await _memberRepo.IsAMember(userId, workspaceId);
    
                var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
                if (board.IsClosed && !isOwner && userTokenRole != "Admin")
                {
                    return StatusCode(403, "The board is closed");
                }
                if(isMember || userTokenRole == "Admin"){
                    
                var boardActivity = boardActivityDto.ToBoardActivityFromCreate(userId);
                await _boardActivityRepo.CreateBoardActivityAsync(boardActivity);
                return CreatedAtAction(nameof(GetBoardActivityById), 
                    new { id = boardActivity.BoardActivityId }, boardActivity);
                    
            }

            return StatusCode(401, "You are not authorized");
            
        }catch(Exception e){
            return StatusCode(500, "Internal server error! " + e.Message);
        }

    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetAllBoardActivity")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllBoardActivity(){

        try{

            var board = await _boardRepo.GetAllBoardsAsync();
            if(board == null){
                return NotFound("Board Not Found!");
            }

            var boardActivity = await _boardActivityRepo.GetAllBoardActivityAsync();
            if(boardActivity.Count == 0){

                return Ok(new List<BoardActivityDto>());
            }

            return Ok(boardActivity);

        }catch(Exception e){
            return StatusCode(500, "Internal server code! " + e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetBoardActivityById")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetBoardActivityById(int boardId){
        
        try{
            var boardActivity = await _boardActivityRepo.GetBoardActivityByIdAsync(boardId);
            if(boardActivity == null){
                return NotFound("Board Activity Not Found!");
            }

            var board = await _boardRepo.GetBoardByIdAsync(boardActivity.BoardId);
            if(board == null){
                return NotFound("Board Not Found!");
            }

            return Ok(boardActivity);

        }catch(Exception e){
            return StatusCode(500, "Internal server error! " + e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetBoardActivityByBoardId")]

    public async Task<IActionResult> GetBoardActivityByBoardId(BoardIdDto boardIdDto){

        try{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var board = await _boardRepo.GetBoardByIdAsync(boardIdDto.BoardId);
            if(board == null){
                return NotFound("Board Not Found!");
            }
            if(userId == null){
                return NotFound("User Not Found!");
            }

            var workspaceId = board.WorkspaceId;
            
            var isMember = await _memberRepo.IsAMember(userId, workspaceId);
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if (board.IsClosed && !isOwner && userTokenRole != "Admin")
            {
                return StatusCode(403, "The board is closed");
            }

            if(isMember || userTokenRole == "Admin"){

                var boardActivity = await _boardActivityRepo.GetBoardActivityByBoardId(boardIdDto.BoardId);
                if(boardActivity.Count == 0){

                    return Ok(new List<BoardActivityDto>());
                }
                return Ok(boardActivity);
            }

            return StatusCode(401, "You are not authorized!");


        }catch(Exception e){
            return StatusCode(500, "Internal server error! " + e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetBoardActivityByWorkspaceId")]
    public async Task<IActionResult> GetBoardActivityByWorkspaceId(int workspaceId){
        try{

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(workspaceId);
            if (workspace == null){
                return NotFound("Workspace Not Found.");
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var board = await _boardRepo.GetBoardByIdAsync(workspaceId);
            if(board == null){
                return NotFound("Board Not Found!");
            }
            if(userId == null){
                return NotFound("User Not Found!");
            }

            var boardActivity = await _boardActivityRepo.GetBoardActivityByWorkspaceIdAsync(workspaceId);
            if (boardActivity == null || !boardActivity.Any()){
                return NotFound("No board activities found for this workspace.");
            }

            return Ok(boardActivity);

        }catch (Exception e){
            return StatusCode(500, "Internal server error!" + e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteBoardActivityById")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteBoardActivityById(BoardActivityIdDto boardActivityIdDto){
        try{

            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var boardActivity = await _boardActivityRepo.GetBoardActivityByIdAsync(boardActivityIdDto.BoardActivityId);
            if(boardActivity == null){
                return NotFound("Board Activity Not Found!");
            }

            var board = await _boardRepo.GetBoardByIdAsync(boardActivity.BoardId);
            if(board == null){
                return NotFound("Board Not Found!");
            }

            var boardDelete = await _boardActivityRepo.DeleteBoardActivityByIdAsync(boardActivityIdDto.BoardActivityId);
            if(boardDelete == null){
                return NotFound("Board Activity Not Found!");
            }

            return Ok("Board Activity Deleted!");


        }catch(Exception e){
            return StatusCode(500, "Internal server error! " + e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteBoardActivityByBoardId")]

    public async Task<IActionResult> DeleteBoardActivityByBoardId (BoardIdDto boardIdDto){
        try{

            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            var board = await _boardRepo.GetBoardByIdAsync(boardIdDto.BoardId);
            if(board == null){
                return NotFound("Board Not Found!");
            }
            if(userId == null){
                return NotFound("User Not Found!");
            }

           var workspaceId = board.WorkspaceId;
            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceId);
            if(isOwner || userTokenRole == "Admin"){
                
                var boardActivity = await _boardActivityRepo.GetBoardActivityByBoardId(boardIdDto.BoardId);
                if(boardActivity.Count == 0){
                    return Ok(new List<BoardActivityDto>());
                }
                return Ok(boardActivity);
            }

            return StatusCode(401, "You are not authorized!");
        }catch(Exception e){
            return StatusCode(404, "Internal Server Error! "+e.Message);
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteBoardActivityByWorkspaceId")]
    public async Task<IActionResult> DeleteBoardActivityByWorkspaceId(WorkspaceIdDto workspaceIdDto){
        
        try{
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var board = await _boardRepo.GetBoardByIdAsync(workspaceIdDto.WorkspaceId);
            if(board == null){
                return NotFound("Board Not Found!");
            }

            var workspace = await _workspaceRepo.GetWorkspaceByIdAsync(workspaceIdDto.WorkspaceId);
            if (workspace == null){
                return NotFound("Workspace Not Found.");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (userId == null){
                return Unauthorized("User Not Found.");
            }

            var isOwner = await _userRepo.UserOwnsWorkspace(userId, workspaceIdDto.WorkspaceId);
            if (!isOwner && userTokenRole != "Admin"){
                return StatusCode(401, "You are not authorized to delete board activities.");
            }


            var boardActivities = await _boardActivityRepo.DeleteBoardActivityByWorkspaceId(workspaceIdDto.WorkspaceId);
            // nese nuk osht gjet naj aktivitet ose nuk este fshi, kthen nje empty list
            if (boardActivities == null || boardActivities.Count == 0){
                return Ok(new List<BoardActivityDto>());
            }

            return Ok(boardActivities);
        }catch (Exception e){
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }

    }

}