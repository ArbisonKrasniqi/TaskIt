using Application.Dtos.TokenDtos;
using Application.Dtos.UserDtos;

namespace Application.Services.Token;

public interface ITokenService
{
    string CreateToken(Domain.Entities.User user);
    string CreateRefreshToken(string userId);
    Task<RefreshTokenDto> RefreshToken(RequestRefreshDto requestRefreshDto);
    bool ValidateRefreshExpiration(string RefreshToken);
}