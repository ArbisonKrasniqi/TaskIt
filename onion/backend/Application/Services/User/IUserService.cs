using Application.Dtos.TokenDtos;
using Application.Dtos.UserDtos;

namespace Application.Services.User;

public interface IUserService
{
    Task<GetUserDto> Register(RegisterDto registerDto);
    Task<RefreshTokenDto> Login(LoginDto loginDto);
    Task<List<GetUserDto>> GetAllUsers();
    Task<GetUserDto> GetUserById(string userId);
    Task<GetUserDto> GetUserByEmail(string email);
    Task<List<UserInfoDto>> SearchUsers(string query);
    Task<GetUserDto> EditUser(UserInfoDto userInfoDto);
    Task<GetUserDto> UpdatePassword(EditUserPasswordDto editUserPasswordDto);
    Task<GetUserDto> ChangePassword(ChangeUserPasswordDto changeUserPasswordDto);
    Task<GetUserDto> UpdateRole(EditUserRoleDto editUserRoleDto);
    Task<GetUserDto> DeleteUser(UserIdDto userIdDto);
}