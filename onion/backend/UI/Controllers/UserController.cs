using System.Linq.Expressions;
using Application;
using Application.Dtos.TokenDtos;
using Application.Dtos.UserDtos;
using Application.Services.Token;
using Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    [HttpPut("refreshToken")]
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
    
    [HttpPut("adminUpdateUser")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> EditUser(EditUserInfoDto editUserInfoDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = await _userService.EditUser(editUserInfoDto);
    
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("adminUpdatePassword")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UpdatePassword(EditUserPasswordDto editUserPasswordDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userService.UpdatePassword(editUserPasswordDto);

            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    [HttpPut("changePassword")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> ChangePassword(ChangeUserPasswordDto changeUserPasswordDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userService.ChangePassword(changeUserPasswordDto);

            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("adminUpdateRole")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateRole(EditUserRoleDto editUserRoleDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userService.UpdateRole(editUserRoleDto);

            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("adminDeleteUserById")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteUserById(UserIdDto userIdDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = await _userService.DeleteUser(userIdDto);

            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}