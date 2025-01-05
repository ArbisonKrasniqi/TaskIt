using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dtos.TokenDtos;
using Application.Services.Utility;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Token;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly IConfiguration _config;
    private readonly IUtilityService _utilityService;
    private readonly IUserRepository _userRepository;
    
    public TokenService(IConfiguration configuration, IUtilityService utilityService, IUserRepository userRepository)
    {
        _config = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        _utilityService = utilityService;
        _userRepository = userRepository;
    }
    public string CreateToken(Domain.Entities.User user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id),
            new Claim("Name", user.FirstName + " " + user.LastName),
            new Claim("Email", user.Email),
            new Claim("Role", user.Role)
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(20),
            SigningCredentials = creds,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string CreateRefreshToken(string userId)
    {
        DateTime expiryDate = DateTime.UtcNow.AddDays(30);
        
        var tokenGuid = _utilityService.GenerateGuid();
        string refreshToken = $"{expiryDate:yyyy-MM-ddTHH:mm:ss}|{tokenGuid}|{userId}";
        return refreshToken;
    }

    public async Task<RefreshTokenDto> RefreshToken(RequestRefreshDto requestRefreshDto)
    {
        if (!ValidateRefreshExpiration(requestRefreshDto.RefreshToken)) throw new Exception("Refresh token has expired");
        var tokenParts = requestRefreshDto.RefreshToken.Split('|');
    
        if (tokenParts.Length < 3) throw new Exception("Invalid refresh token format.");
        var userId = tokenParts[2];

        var users = await _userRepository.GetUsers(userId: userId);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("Refresh Token is invalid");

        var tokenValid = _utilityService.VerifyHash(requestRefreshDto.RefreshToken, user.RefreshToken);
        if (!tokenValid) throw new Exception("Refresh Token is invalid");
        
        var token = CreateToken(user);
        var newRefreshToken = CreateRefreshToken(userId);
        var newHashedToken = _utilityService.GenerateHash(newRefreshToken);
        
        user.RefreshToken = newHashedToken;
        await _userRepository.UpdateUser(user);

        return new RefreshTokenDto(token, newRefreshToken);
    }

    public bool ValidateRefreshExpiration(string RefreshToken)
    {
        var parts = RefreshToken.Split('|');

        if (parts.Length < 3)
        {
            return false;
        }

        if (!DateTime.TryParse(parts[0], out DateTime expiryDate))
        {
            return false;
        }
        
        return DateTime.UtcNow <= expiryDate;
    }
}