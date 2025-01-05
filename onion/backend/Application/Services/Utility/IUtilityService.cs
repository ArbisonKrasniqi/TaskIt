namespace Application.Services.Utility;

public interface IUtilityService
{
    string GenerateHash(string value);
    bool VerifyHash(string value, string hash);
    string GenerateGuid();
}