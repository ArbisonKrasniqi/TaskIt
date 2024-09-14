using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using backend.Interfaces;
using backend.Models;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace backend.Service;

public class TokenService : ITokenService
{
    //IConfiguration is used to access appsettings.json
    private readonly IConfiguration _config;
    
    //_key will be used for signing the Tokens
    private readonly SymmetricSecurityKey _key;
    
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
    }

    public string CreateToken(User user, string role)
    {
        //The claims that the JWT will include
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id),
            new Claim("Name", user.FirstName+" "+user.LastName),
            new Claim("Email", user.Email),
            new Claim("Role", role),
        };

        //What type of security algorithm will be used
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddSeconds(20),
            SigningCredentials = creds,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };

        //tokenHandler is used to create tokens
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        //WriteToken(token) turns the token from an object datatype to string
        return tokenHandler.WriteToken(token);
    }
    
    public RefreshToken GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.Now.AddDays(30),
            UserId = user.Id
        };
        
        
        
        return refreshToken;
    }
}