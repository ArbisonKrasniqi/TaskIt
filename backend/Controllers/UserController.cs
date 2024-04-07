using backend.DTOs.User;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    UserName = registerDto.Email
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
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
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
    }
