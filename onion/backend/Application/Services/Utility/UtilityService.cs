using BCrypt.Net;

namespace Application.Services.Utility;

public class UtilityService : IUtilityService
{
    public string GenerateHash(string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value);
    }

    public bool VerifyHash(string value, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(value, hash);
    }
    public string GenerateGuid()
    {
        return Guid.NewGuid().ToString();
    }
}