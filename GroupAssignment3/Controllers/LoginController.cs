using GroupAssignment3.Dto;
using GroupAssignment3.Helper;
using GroupAssignment3.Identity;
using GroupAssignment3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GroupAssignment3.Controllers.UserApiController;

namespace GroupAssignment3.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly UserService userService;

        public LoginController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("v1/auth/login")]
        public IActionResult DoLogin([FromBody] UserDto loginUser)
        {
            var authUser = this.userService.AuthenticateUser(loginUser);
            // create JWT
            var jwtToken = JwtHelper.CreateUserToken(authUser.userId, authUser.email!, false);
            return Ok(RestFormat.UserEntity(authUser, jwtToken));
        }

        [HttpPost("v1/auth/signup")]
        public IActionResult CreateUser([FromBody] UserCreateDto userCreate)
        {
            UserEntity createdUser = this.userService.CreateUser(userCreate);
            return Ok(RestFormat.UserEntity(createdUser));
        }

        [HttpPost("v1/auth/admin/login")]
        public IActionResult DoAdminLogin([FromBody] UserDto loginUser)
        {
            var jwtToken = JwtHelper.CreateUserToken(loginUser.username, "admin@conestogac.on.ca", true);
            return Ok(new
            {
                Token = jwtToken
            });
        }

    }
}
