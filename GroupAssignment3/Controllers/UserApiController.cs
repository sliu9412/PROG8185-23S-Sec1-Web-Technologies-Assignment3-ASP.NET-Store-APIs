
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GroupAssignment3.Dto;
using GroupAssignment3.Services;
using GroupAssignment3.Identity;
using GroupAssignment3.Helper;
using Microsoft.AspNetCore.Http;

namespace GroupAssignment3.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserApiController : ControllerBase
    {

        private readonly UserService userService;
        private readonly IHttpContextAccessor httpContext;

        public UserApiController(UserService userService, IHttpContextAccessor httpContext)
        {
            this.userService = userService;
            this.httpContext = httpContext;
        }

        [HttpGet("v1/user")]
        [Authorize(Policy = IdentityData.ScopeAdminPolicyName)]
        public IActionResult GetUsers()
        {
            return Ok(this.userService.GetAllUsers()
                .Select(u => RestFormat.UserEntity(u)));
        }

        [HttpGet("v1/user/{userId}")]
        public IActionResult GetUserById(string userId)
        {
            JwtHelper.ValidateUserIDByScope(this.httpContext, userId);
            return Ok(RestFormat.UserEntity(this.userService.GetUserById(userId)));
        }

        [HttpGet("v1/user/mydata")]
        [Authorize(Policy = IdentityData.ScopeUserPolicyName)]
        public IActionResult CurrentUser()
        {
            string userId = JwtHelper.GetSessionFromToken(this.httpContext).userId;
            return Ok(RestFormat.UserEntity(this.userService.GetUserById(userId)));
        }

        [HttpPost("v1/user")]
        [Authorize(Policy = IdentityData.ScopeAdminPolicyName)]
        public IActionResult AdminCreateUser([FromBody] UserCreateDto createUser)
        {
            UserEntity createdUser = this.userService.CreateUser(createUser);
            return Ok(RestFormat.UserEntity(createdUser));
        }

        [HttpPut("v1/user")]
        public IActionResult UserModify([FromBody] UserUpdateDto updateUser)
        {
            JwtHelper.ValidateUserIDByScope(this.httpContext, updateUser.id);
            UserEntity updatedUser = this.userService.UpdateUser(updateUser);
            return Ok(RestFormat.UserEntity(updatedUser));
        }

        [HttpDelete("v1/user/{userId}")]
        public IActionResult DeleteUserByID(String userId)
        {
            JwtHelper.ValidateUserIDByScope(this.httpContext, userId);
            UserEntity deletedUser = this.userService.DeleteUser(userId);
            return Ok(RestFormat.UserEntity(deletedUser));
        }

        
    }
}