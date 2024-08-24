using System.Text.RegularExpressions;
using backend.DTOs.User;
using backend.DTOs.User.Input;
using backend.DTOs.User.Output;
using backend.Interfaces;
using backend.Mappers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
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
        
        //ITokenRepo to implement refresh tokens
        private readonly ITokenRepository _tokenRepo;
        private readonly IUserRepository _userRepo;
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, ITokenRepository tokenRepo, IUserRepository userRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _tokenRepo = tokenRepo;
            _userRepo = userRepo;
        }
        
        
        //LOGIN AND REGISTER
        //Register as a normal user
        [HttpPost("register")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
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
                    DateCreated = DateTime.Now
                };

                
                //Create user using CreateAsync
                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);
                if (createdUser.Succeeded)
                {
                        return Ok("User created!");
                }
                return BadRequest(createdUser.Errors);
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
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                //Find user that matches email
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
                if (user == null) return Unauthorized("Invalid username!");

                var rolesResult = await _userManager.GetRolesAsync(user);
                var role = "User";
                if (rolesResult.Count != 0)
                {
                    role = "Admin";
                } 
            
                //Check password with found user
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect!");

                var refreshToken = _tokenService.GenerateRefreshToken(user);
                
                //Add associated refresh token to user.
                //If user already has refresh token, this overwrites it.
                await _tokenRepo.AddRefreshToken(refreshToken);
                
                return Ok
                (
                    new LoggedInDTO
                    {
                        accessToken = _tokenService.CreateToken(user, role),
                        refreshToken = refreshToken.Token
                    }
                ); 
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        
        //ADMIN API CALLS
        [HttpPost("adminCreate")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
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
                    DateCreated = DateTime.Now
                };
                
                //Create user using CreateAsync
                var createdUser = await _userManager.CreateAsync(user, createUserDto.Password);
                if (!createdUser.Succeeded) return StatusCode(500, createdUser.Errors);

                if (createUserDto.IsAdmin)
                {
                    var adminResult = await _userManager.AddToRoleAsync(user, "Admin");
                    if (adminResult.Succeeded)
                    {
                        return Ok(UserMappers.ToGetUserDTO(user, "Admin"));
                    }

                    return StatusCode(500, "User created without admin role");
                }

                return Ok(UserMappers.ToGetUserDTO(user, "User"));

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        
        [HttpGet("adminAllUsers")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                // Get a list of all users
                var users = await _userManager.Users.ToListAsync();
                var usersDto = new List<GetUserDTO>();
                foreach (var user in users)
                {
                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    var userDto = UserMappers.ToGetUserDTO(user, isAdmin ? "Admin" : "User");
                    usersDto.Add(userDto);
                }

                return Ok(usersDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("adminAllAdmins")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var admins = new List<User>();

                foreach (var user in users)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        var newAdmin = new User
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            DateCreated = user.DateCreated
                        };
                        admins.Add(newAdmin);
                    }
                }

                var adminsDto = admins.Select(admin => UserMappers.ToGetUserDTO(admin, "Admin"));
                return Ok(adminsDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
        }
        
        [HttpGet("adminUserID")]
        [Authorize(AuthenticationSchemes = "Bearer")]
       // [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (!ModelState.IsValid) return BadRequest("Id cannot be empty");

            try
            {
                //Find specific user
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return StatusCode(404, "User does not exist!");
                }

                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                if (isAdmin)
                {
                    return Ok(UserMappers.ToGetUserDTO(user, "Admin"));
                }
                return Ok(UserMappers.ToGetUserDTO(user, "User"));
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
        }
        
        [HttpGet("adminUserEmail")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserByEmail(string emailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Wrong parameters");
            }

            try
            {
                //Find specific user
                var user = await _userManager.FindByEmailAsync(emailDto);
                if (user == null)
                {
                    return StatusCode(404, "Email does not exist!");
                }

                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin)
                {
                    return Ok(UserMappers.ToGetUserDTO(user, "Admin"));
                }
                return Ok(UserMappers.ToGetUserDTO(user, "User"));
                
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
            
        }
        
        [HttpPut("adminUpdateUser")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditUser(EditUserDTO editUserDto)
        {
                //Check if valid ModelState(DTO) and if the role is either Admin or User
                if (!ModelState.IsValid) return BadRequest(ModelState);

                try
                {
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
                            //Conflict error code
                            return BadRequest("New email already exists!");
                        }
                    }
                    
                    //Apply changes
                    var editResult = await _userManager.UpdateAsync(user);
                    if (editResult.Succeeded)
                    {
                        return Ok("User successfully updated");
                    }

                    return StatusCode(500, "User could not be updated!");
                }
                catch (Exception e)
                {
                    return StatusCode(500, e);
                }

        }

        [HttpPut("adminUpdatePassword")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdatePassword(EditUserPasswordDTO editUserPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Wrong Parameters");

            try
            {
                //Check if user first exists.
                var user = await _userManager.FindByIdAsync(editUserPasswordDto.Id);
                if (user == null) return BadRequest("User does not exist!");

                //Check if new password is valid

                bool isValidPassword = Regex.IsMatch(editUserPasswordDto.Password,
                    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$");

                if (isValidPassword)
                {
                    var newHash = _userManager.PasswordHasher.HashPassword(user, editUserPasswordDto.Password);
                    if (user.PasswordHash != newHash)
                    {
                        user.PasswordHash = newHash;
                    }
                }
                else
                {
                    return BadRequest("New password is not secure!");
                }

                //Apply changes
                var editResult = await _userManager.UpdateAsync(user);
                if (editResult.Succeeded)
                {
                    return Ok("Password successfully udpated!");
                }

                return StatusCode(500, "Password could not be changed!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

       


        [HttpPut("adminUpdateRole")]
        [Authorize(AuthenticationSchemes = "Bearer")]
       // [Authorize(Policy = "AdminOnly")]

        public async Task<IActionResult> UpdateRole(EditRoleDTO editRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Wrong parameters");

            var wantedRole = editRoleDto.isAdmin ? "Admin" : "User";

            try
            {
                var user = await _userManager.FindByIdAsync(editRoleDto.Id);
                if (user == null) return StatusCode(404, "User not found");

                //If no roles edge case, add User role, and if wanted, add admin role.
                var checkRole = await _userManager.GetRolesAsync(user);
                if (checkRole.IsNullOrEmpty())
                {
                    if (wantedRole == "User")
                        return Ok(UserMappers.ToGetUserDTO(user, "User"));

                    if (wantedRole == "Admin")
                    {
                        var addRoleAdmin = await _userManager.AddToRoleAsync(user, "Admin");
                        if (addRoleAdmin.Succeeded)
                        {
                            return Ok(UserMappers.ToGetUserDTO(user, "Admin"));
                        }

                        return StatusCode(500, "Role could not be updated");

                    }
                }


                //If user has desired role, do not make any changes.
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (wantedRole == "Admin")
                {
                    if (currentRoles.Any(currentRole => currentRole == "Admin"))
                    {
                        return Ok(UserMappers.ToGetUserDTO(user, "Admin"));
                    }
                }

                if (wantedRole == "User")
                {
                    if (currentRoles.All(currentRole => currentRole != "Admin"))
                    {
                        return Ok(UserMappers.ToGetUserDTO(user, "User"));
                    }
                }

                switch (wantedRole)
                {
                    case "User":
                        var removeAdmin = await _userManager.RemoveFromRoleAsync(user, "Admin");
                        if (removeAdmin.Succeeded)
                        {
                            return Ok(UserMappers.ToGetUserDTO(user, "User"));
                        }

                        return StatusCode(500, "Role could not be updated");

                    case "Admin":
                        var addAdmin = await _userManager.AddToRoleAsync(user, "Admin");
                        if (addAdmin.Succeeded)
                        {
                            return Ok(UserMappers.ToGetUserDTO(user, "Admin"));
                        }

                        return StatusCode(500, "Role could not be updated");

                    default:
                        return BadRequest("Wrong parameters");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
        }

        [HttpDelete("adminDeleteUserById")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUserById(UserIdDTO userIdDto)
        {
            if (userIdDto.id.IsNullOrEmpty()) return BadRequest("Id cannot be empty!");

            try
            {
                //Find specific user using id
                var user = await _userManager.FindByIdAsync(userIdDto.id);
                if (user == null) return StatusCode(404, "User does not exist!");

                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                //Delete found user
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    if (isAdmin)
                    {
                        return Ok(UserMappers.ToGetUserDTO(user, "Admin"));
                    }
                    return Ok(UserMappers.ToGetUserDTO(user, "User"));
                }

                return StatusCode(500, "User could not be deleted");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
        }
        
        //SEARCH
        [HttpGet("search")]
        public async Task<ActionResult<List<SearchUserDto>>> SearchUsers([FromQuery] string query)
        {
            var result = await _userRepo.SearchUsersAsync(query);
            return Ok(result);
        }
        
    }
