using System.Net;
using System.Text.RegularExpressions;
using backend.Data;
using backend.DTOs.User;
using backend.Interfaces;
using backend.Mappers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers;

    [ApiController]
    [Route("backend/user")]
    public class UserController : ControllerBase
    {
        
        //UserManager<User> is similar to the repositories we create to access the database.
        //It comes with its own functions for users.
        private readonly UserManager<User> _userManager;
        
        //SignInManager is used to authenticate the user when logging in
        private readonly SignInManager<User> _signInManager;

        //ITokenService is used to implement JWT in the user API
        private readonly ITokenService _tokenService;
        
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        
        
        //LOGIN AND REGISTER
        //Register as a normal user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                
                //Create User from DTO
                var user = new User
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    Role = "User"
                };

                //Create user using CreateAsync
                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    //Add "User" role to new user
                    var roleResult = await _userManager.AddToRoleAsync(user, "User"); ;
                    if (roleResult.Succeeded)
                    {
                        return Ok("User created");
                    }
                    return StatusCode(500, roleResult.Errors);
                }
                return StatusCode(500, createdUser.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        //Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Find user that matches email
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null) return Unauthorized("Invalid username");
            
            //Check password with found user
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect!");
            return Ok
            (
                new NewUserDTO
                {
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }
        
        
        //ADMIN API CALLS
        [HttpPost("adminCreate")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(CreateUserDTO createUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //Create new user model
                var user = new User
                {
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    Email = createUserDto.Email,
                    UserName = createUserDto.Email,
                    Role = createUserDto.IsAdmin ? "Admin" : "User"
                };
                
                //Create user using CreateAsync
                var createdUser = await _userManager.CreateAsync(user, createUserDto.Password);
                if (!createdUser.Succeeded) return StatusCode(500, createdUser.Errors);
                
                //Add the role. User role is always added by default
                var userRoleResult = await _userManager.AddToRoleAsync(user, "User");
                if (userRoleResult.Succeeded)
                {
                    //If new user is admin, add admin role
                    if (createUserDto.IsAdmin)
                    {
                        var adminRoleResult = await _userManager.AddToRoleAsync(user, "User");
                        if (adminRoleResult.Succeeded)
                        {
                            return Ok("User created");
                        }
                    }
                    //If new user is NOT admin just return Ok
                    return Ok("User created");
                    
                }
                return StatusCode(500, "Could not create user");

            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
        
        
        [HttpGet("adminAllUsers")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                //Get a list of all users
                var users = await _userManager.Users.ToListAsync();
                
                //Turn every user in list into GetUserDTO
                var usersDto = users.Select(user => UserMappers.ToGetUserDTO(user));
                return Ok(usersDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
           
        }
        
        [HttpGet("adminUserID")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserById(string id)
        {
            //Find specific user
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return StatusCode(404, "User does not exist!");
            }

            return Ok(user);
        }
        
        [HttpGet("adminUserEmail")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            //Find specific user
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StatusCode(404, "Email does not exist!");
            }
            
            return Ok(user);
        }
        
        [HttpPut("adminUpdateUser")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditUser(EditUserDTO editUserDto)
        {
            try
            {
                //Check if valid ModelState(DTO) and if the role is either Admin or User
                if (!ModelState.IsValid && editUserDto.Role != "Admin" && editUserDto.Role != "User")
                {
                    return BadRequest("Parameters incorrect!");
                }

                //Check user that is being edited if it exists.
                var user = await _userManager.FindByIdAsync(editUserDto.Id);
                if (user == null) return BadRequest("User does not exist!");

                //Change parameters
                if (user.FirstName != editUserDto.FirstName) user.FirstName = editUserDto.FirstName;

                if (user.LastName != editUserDto.LastName) user.LastName = editUserDto.LastName;
                
                //Check if the email is being changed
                if (user.Email != editUserDto.Email)
                {
                    //If email is being changed, check if it exists for another user
                    var emailResult = await _userManager.FindByEmailAsync(editUserDto.Email);
                    if (emailResult == null)
                    {
                        //UserName and Email are the same in our project
                        user.Email = editUserDto.Email;
                        user.UserName = editUserDto.Email;
                    }
                    else
                    {
                        return StatusCode(409, "Email already exists!");
                    }
                }


                //Check if new password is valid
                bool isValidPassword = Regex.IsMatch(editUserDto.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$");
                if (isValidPassword)
                {
                    //If new password is valid, replace current user PasswordHash
                    var newHash = _userManager.PasswordHasher.HashPassword(user, editUserDto.Password);
                    if (user.PasswordHash != newHash)
                    {
                        user.PasswordHash = newHash;
                    }
                }
                else
                {
                    return StatusCode(500, "New password is not secure!");
                }

                //Apply current changes (Role is not changed yet)
                var editResult = await _userManager.UpdateAsync(user);
                
                if (editResult.Succeeded)
                {
                    //First check if role list for user is empty
                    var checkRole = await _userManager.GetRolesAsync(user);
                    if (checkRole.IsNullOrEmpty())
                    {
                        //If its empty, just add the desired role
                        var addRole = await _userManager.AddToRoleAsync(user, editUserDto.Role);
                        if (addRole.Succeeded)
                        {
                            //Apply role attribute
                            user.Role = editUserDto.Role;
                            var update = await _userManager.UpdateAsync(user);
                            if (update.Succeeded)
                            {
                                return Ok("User successfully updated!");
                            }
                        }
                        else
                        {
                            return StatusCode(500, "Could not update user role!");
                        }
                    }

                    //If current role and desired role are the same, dont do any changes.
                    if (user.Role == editUserDto.Role)
                    {
                        return Ok("User successfully updated");
                    }

                    //If they are different and current role is "User" that means desired role is "Admin"
                    if (user.Role == "User")
                    {
                        //Add user to "Admin" role
                        var addAdmin = await _userManager.AddToRoleAsync(user, editUserDto.Role);
                        if (addAdmin.Succeeded)
                        {
                            //Apply role attribute
                            user.Role = editUserDto.Role;
                            var update = await _userManager.UpdateAsync(user);
                            if (update.Succeeded)
                            {
                                return Ok("User successfully updated!");
                            }
                        }

                        return StatusCode(500, "Could not update user role!");
                    }

                    //If they are different and current role is "Admin" that means desired role is "User"
                    if (user.Role == "Admin")
                    {
                        //Just remove user from Admin role.
                        var removeAdmin = await _userManager.RemoveFromRoleAsync(user, editUserDto.Role);
                        if (removeAdmin.Succeeded)
                        {
                            //Apply role attribute
                            user.Role = editUserDto.Role;
                            var update = await _userManager.UpdateAsync(user);
                            if (update.Succeeded)
                            {
                                return Ok("User successfully updated!");
                            }
                        }

                        return StatusCode(500, "Could not update user role!");
                    }
                }

                return StatusCode(500, "User could not be updated!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
        
        [HttpDelete("adminDeleteUserById")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            //Semi-Validate the user id
            if (string.IsNullOrEmpty(id)) return BadRequest("Invalid user Id");
            
            //Find specific user using id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest("User does not exist");

            //Delete found user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted");
            }

            return StatusCode(500, result.Errors);
        }
    }
