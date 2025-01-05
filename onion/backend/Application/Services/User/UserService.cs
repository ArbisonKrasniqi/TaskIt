using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Application.Dtos.TokenDtos;
using Application.Dtos.UserDtos;
using Application.Services.Token;
using Application.Services.Utility;
using Domain.Interfaces;

namespace Application.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUtilityService _utilityService;
    private readonly ITokenService _tokenService;

    public UserService(IUtilityService utilityService, IUserRepository userRepository, ITokenService tokenService)
    {
        _utilityService = utilityService;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<GetUserDto> Register(RegisterDto registerDto)
    {
        var newId = _utilityService.GenerateGuid();
        var hashPassword = _utilityService.GenerateHash(registerDto.Password);
        var newUser = new Domain.Entities.User(
            newId, //Guid
            registerDto.FirstName,
            registerDto.LastName,
            registerDto.Email,
            hashPassword,
            DateTime.Now, //DateCreated
            false, //IsDeleted
            "User", //Role
            "" //RefreshToken
        );

        var addedUser = await _userRepository.CreateUser(newUser);
        return new GetUserDto(newUser);
    }

    public async Task<RefreshTokenDto> Login(LoginDto loginDto)
    {
        //Find user that matches email
        var users = await _userRepository.GetUsers(email: loginDto.Email);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");
        if (user.IsDeleted) throw new Exception("User is deleted");
            
        //Check password with found user
        var passwordValid = _utilityService.VerifyHash(loginDto.Password, user.PasswordHash);
        if (!passwordValid) throw new Exception("Invalid password");
        
        //Create refresh token and update user with hashed token
        var refreshToken = _tokenService.CreateRefreshToken(user.Id);
        user.RefreshToken = _utilityService.GenerateHash(refreshToken);
        await _userRepository.UpdateUser(user);
        
        var accessToken = _tokenService.CreateToken(user);
        return new RefreshTokenDto(accessToken, refreshToken);
    }

    public async Task<List<GetUserDto>> GetAllUsers()
    {
        var users = await _userRepository.GetUsers();
        var usersDto = new List<GetUserDto>();
        foreach (var user in users)
        {
            usersDto.Add(new GetUserDto(user));
        }

        return usersDto;
    }

    public async Task<GetUserDto> GetUserById(string userId)
    {
        var users = await _userRepository.GetUsers(userId: userId, isDeleted: false);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");
        
        return new GetUserDto(user);
    }

    public async Task<GetUserDto> GetUserByEmail(string email)
    {
        var users = await _userRepository.GetUsers(email: email, isDeleted: false);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");
        
        return new GetUserDto(user);
    }

    public async Task<List<UserInfoDto>> SearchUsers(string query)
    {
        //SOSHT EFFICIENT AMO KRYN PUN MA MIR MOS ME NDRRU REPOSITORY
        var usersFromSearch1 = await _userRepository.GetUsers(firstName: query, isDeleted:false);
        var usersFromSearch2 = await _userRepository.GetUsers(lastName: query, isDeleted:false);
        var usersFromSearch3 = await _userRepository.GetUsers(email: query, isDeleted:false);
        
        var allUsers = usersFromSearch1.Concat(usersFromSearch2).Concat(usersFromSearch3);
        var distinctUsers = allUsers.GroupBy(u => u.Id).Select(g => g.First());

        var userInfoList = new List<UserInfoDto>();
        foreach (var user in distinctUsers)
        {
            userInfoList.Add(new UserInfoDto(user));
        }

        return userInfoList;
    }

    public async Task<GetUserDto> EditUser(UserInfoDto userInfoDto)
    {
        var users = await _userRepository.GetUsers(userId: userInfoDto.Id);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");
        if (user.IsDeleted) throw new Exception("User is deleted");

        var emailUsers = await _userRepository.GetUsers(email: userInfoDto.Email);
        var emailUser = emailUsers.FirstOrDefault();
        if (emailUser != null) throw new Exception("New email already in use");
        
        user.FirstName = userInfoDto.FirstName;
        user.LastName = userInfoDto.LastName;
        user.Email = userInfoDto.Email;
        
        var updatedUser = await _userRepository.UpdateUser(user);
        return new GetUserDto(updatedUser);
    }

    public Task<GetUserDto> UpdatePassword(EditUserPasswordDto editUserPasswordDto)
    {
        throw new NotImplementedException();
    }

    public Task<GetUserDto> ChangePassword(ChangeUserPasswordDto changeUserPasswordDto)
    {
        throw new NotImplementedException();
    }

    public Task<GetUserDto> UpdateRole(EditUserRoleDto editUserRoleDto)
    {
        throw new NotImplementedException();
    }

    public Task<GetUserDto> DeleteUser(UserIdDto userIdDto)
    {
        throw new NotImplementedException();
    }
}