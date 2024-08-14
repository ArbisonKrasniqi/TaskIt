using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using backend.DTOs.RefreshTokens;
using backend.DTOs.RefreshTokens.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace backend.Controllers;

[ApiController]
[Route("backend/token")]
//[Authorize(AuthenticationSchemes = "Bearer")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ITokenRepository _tokenRepo;
    private readonly UserManager<User> _userManager;

    public TokenController(ITokenService tokenService, ITokenRepository tokenRepo, UserManager<User> userManager)
    {
        _tokenRepo = tokenRepo;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    
    //Ky api thirret sa here qe nevojitet te behet refresh token i userit i cili eshte logged in.
    [HttpPost("refreshToken")]
    public async Task<IActionResult> Refresh(RequestRefreshTokenDTO requestRefreshTokenDto)
    {
        try
        {
            var storedToken = await _tokenRepo.GetRefreshTokenValue(requestRefreshTokenDto.refreshToken);

            if (storedToken == null)
            {
                return Unauthorized("Refresh token is invalid.");
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            if (storedToken.Expires <= DateTime.Now)
            {
                return Unauthorized("Your token has expired.");
            }

            if (storedToken.Token == requestRefreshTokenDto.refreshToken)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                var newAccessToken = _tokenService.CreateToken(user, isAdmin ? "Admin" : "User");
                var newRefreshToken = _tokenService.GenerateRefreshToken(user);

                await _tokenRepo.AddRefreshToken(newRefreshToken);

                var newRefreshTokenDto = new RefreshTokenDTO
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken.Token
                };

                return Ok(newRefreshTokenDto);
            }

            return Unauthorized("Your token is invalid.");
        }
        catch (Exception e)
        {
            // Log the exception message and stack trace here for debugging purposes
            return StatusCode(500, e.Message);
        }
    }
}