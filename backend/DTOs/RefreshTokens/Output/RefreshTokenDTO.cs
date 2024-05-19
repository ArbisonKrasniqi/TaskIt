using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.RefreshTokens;

public class RefreshTokenDTO
{
    public string accessToken { get; set; } = string.Empty;
    public string refreshToken { get; set; } = string.Empty;
}