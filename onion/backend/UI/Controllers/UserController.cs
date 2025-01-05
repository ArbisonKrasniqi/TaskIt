using Application.Dtos.TokenDtos;
using Application.Dtos.UserDtos;
using Application.Services.Token;
using Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;


[ApiController]
[Route("backend/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    
    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = await _userService.Register(registerDto);
            
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tokens = await _userService.Login(loginDto);

            return Ok(tokens);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> Refresh(RequestRefreshDto requestRefreshDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var tokens = await _tokenService.RefreshToken(requestRefreshDto);

            return Ok(tokens);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("adminAllUsers")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
    
    [HttpGet("adminUserID")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("User Id is empty");

            var user = await _userService.GetUserById(userId);
            
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<List<UserInfoDto>>> SearchUsers([FromQuery] string query)
    {
        try
        {
            var result = await _userService.SearchUsers(query);
            return Ok(result);
        } catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    //Error me Dto constructor te UserInfoDto
    // [HttpPut("adminUpdateUser")]
    // [Authorize(AuthenticationSchemes = "Bearer")]
    // public async Task<IActionResult> EditUser(UserInfoDto userInfoDto)
    // {
    //     try
    //     {
    //         if (!ModelState.IsValid) return BadRequest(ModelState);
    //         var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
    //         var userTokenRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
    //         if (userId != userInfoDto.Id && userTokenRole != "Admin") return StatusCode(401, "You are not authorized");
    //
    //         var user = await _userService.EditUser(userInfoDto);
    //
    //         return Ok(user);
    //     }
    //     catch (Exception e)
    //     {
    //         return StatusCode(500, e.StackTrace);
    //     }
    // }
}