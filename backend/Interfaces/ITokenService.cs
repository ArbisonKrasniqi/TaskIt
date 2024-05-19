using System.Security.Claims;
using backend.Models;

namespace backend.Interfaces;

public interface ITokenService
{
    string CreateToken(User user, string role);
    RefreshToken GenerateRefreshToken(User user);
}