using backend.Models;

namespace backend.Interfaces;

public interface ITokenRepository
{
    Task<RefreshToken> GetUserRefreshToken(User user);
    Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
    Task<RefreshToken> RemoveRefreshToken(RefreshToken refreshToken);
}