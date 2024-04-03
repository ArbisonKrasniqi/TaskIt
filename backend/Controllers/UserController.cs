using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        //UserManager<User> is similar to the repositories we create to access the database.
        //It comes with its own functions for users.
        
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
    }
