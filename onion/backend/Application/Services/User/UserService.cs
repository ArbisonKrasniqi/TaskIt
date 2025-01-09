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
    private readonly UserContext _userContext;

    public UserService(IUtilityService utilityService, IUserRepository userRepository, ITokenService tokenService, UserContext userContext)
    {
        _utilityService = utilityService;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _userContext = userContext;
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
        //SOSHT EFIKASE HIQ AMO KRYN PUN MA MIR MOS ME NDRRU REPOSITORY
        var usersFromSearch1 = await _userRepository.GetUsers(firstName: query, isDeleted:false);
        var usersFromSearch2 = await _userRepository.GetUsers(lastName: query, isDeleted:false);
        var usersFromSearch3 = await _userRepository.GetUsers(email: query, isDeleted:false);
        
        //Boji 3 listat bashk edhe hiq duplikatet
        var allUsers = usersFromSearch1.Concat(usersFromSearch2).Concat(usersFromSearch3);
        var distinctUsers = allUsers.GroupBy(u => u.Id).Select(g => g.First());

        //Kthe secilin user ne DTO
        var userInfoList = new List<UserInfoDto>();
        foreach (var user in distinctUsers)
        {
            userInfoList.Add(new UserInfoDto(user));
        }

        return userInfoList;
    }

    public async Task<GetUserDto> EditUser(EditUserInfoDto editUserInfoDto)
    {
        if (_userContext.Id != editUserInfoDto.Id && _userContext.Role != "Admin") throw new Exception("You are not authorized");

        var users = await _userRepository.GetUsers(userId: editUserInfoDto.Id);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");
        if (user.IsDeleted) throw new Exception("User is deleted");

        var emailUsers = await _userRepository.GetUsers(email: editUserInfoDto.Email);
        var emailUser = emailUsers.FirstOrDefault();
        if (emailUser != null) throw new Exception("New email already in use");
        
        user.FirstName = editUserInfoDto.FirstName;
        user.LastName = editUserInfoDto.LastName;
        user.Email = editUserInfoDto.Email;
        
        var updatedUser = await _userRepository.UpdateUser(user);
        return new GetUserDto(updatedUser);
    }

    public async Task<GetUserDto> UpdatePassword(EditUserPasswordDto editUserPasswordDto)
    {
        
        if (_userContext.Id != editUserPasswordDto.Id && _userContext.Role != "Admin") throw new Exception ("You are not authorized");
        
        var users = await _userRepository.GetUsers(userId: editUserPasswordDto.Id);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");

        var newHash = _utilityService.GenerateHash(editUserPasswordDto.Password);
        user.PasswordHash = newHash;

        var updatedUser = await _userRepository.UpdateUser(user);
        return new GetUserDto(updatedUser);
    }

    public async Task<GetUserDto> ChangePassword(ChangeUserPasswordDto changeUserPasswordDto)
    {
        if (_userContext.Id != changeUserPasswordDto.Id && _userContext.Role != "Admin") throw new Exception( "You are not authorized");
        
        var users = await _userRepository.GetUsers(userId: changeUserPasswordDto.Id);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");
        
        var passwordValid = _utilityService.VerifyHash(changeUserPasswordDto.OldPassword, user.PasswordHash);
        if (!passwordValid) throw new Exception("Old password is incorrect");
        
        var newHash = _utilityService.GenerateHash(changeUserPasswordDto.Password);
        user.PasswordHash = newHash;

        var updatedUser = await _userRepository.UpdateUser(user);
        return new GetUserDto(updatedUser);
    }

    public async Task<GetUserDto> UpdateRole(EditUserRoleDto editUserRoleDto)
    {
        var users = await _userRepository.GetUsers(userId: editUserRoleDto.Id);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");
        
        user.Role = editUserRoleDto.IsAdmin ? "Admin" : "User";

        var updatedUser = await _userRepository.UpdateUser(user);
        return new GetUserDto(updatedUser);
    }

    public async Task<GetUserDto> DeleteUser(UserIdDto userIdDto)
    {
        var users = await _userRepository.GetUsers(userId: userIdDto.Id);
        var user = users.FirstOrDefault();
        if (user == null) throw new Exception("User not found");

        user.IsDeleted = !user.IsDeleted;
        var updatedUser = await _userRepository.UpdateUser(user);
        
        return new GetUserDto(updatedUser);
    }
}