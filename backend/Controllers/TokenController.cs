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
        var storedToken = await _tokenRepo.GetRefreshTokenValue(requestRefreshTokenDto.refreshToken);
        
        //Merr user nga databaza bazuar nga id ne jwt
        var user = await _userManager.FindByIdAsync(storedToken.UserId);

        //Check if stored token is expired
        if (storedToken.Expires < DateTime.Now)
        {
            return Unauthorized("Your token has expired");
        }
        
        //Shiko se tokeni i derguar nga front eshte i njejte me ate i ruajtur ne databaze
        //Nese nuk jane, i bie qe tokeni i derguar nga front eshte perdorur me para dhe nuk vlen me.
        
        if (storedToken.Token != null)
        {
            //Nese tokenet jane te njejte atehere gjenero nje JWT token te ri.
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var newAccessToken = _tokenService.CreateToken(user, isAdmin ? "Admin" : "User");
            
            //Gjenero nje refresh token te ri
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);
            //Vendos refresh tokenin e ri ne databaze per ta perdorur per validim heren tjeter qe behet refresh
            await _tokenRepo.AddRefreshToken(newRefreshToken);

            //Dergo tokenet e ri tek useri.
            var newRefreshTokenDto = new RefreshTokenDTO
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken.Token
            };

            return Ok(newRefreshTokenDto);

        }
        return Unauthorized("You token is invalid");
    }
}