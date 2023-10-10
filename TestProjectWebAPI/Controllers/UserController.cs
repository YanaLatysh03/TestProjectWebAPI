using DocumentService.Exceptions;
using DocumentService.Models.Request;
using DocumentService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int limit, int offset, string? orderBy, string? sort, string? filter)
        {
            try
            {
                if (limit < 0 || offset < 0)
                {
                    throw new InvalidDataException("Invalid limit or offset");
                }

                _logger.LogInformation("Request params: Limit = {limit}, Offset = {offset}, OrderBy = {orderBy}, Sort = {sort}, Filter = {filter}",
                    limit, offset, orderBy, sort, filter);

                var result = await _userService.GetAllUsers(limit, offset, orderBy, sort, filter);

                _logger.LogInformation("Result: {@result}", result);

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("user-by-id")]
        public async Task<IActionResult> GetUserById([FromQuery] Guid userId)
        {
            _logger.LogInformation("Request param: UserId = {userId}", userId);
            var result = await _userService.GetUserById(userId);

            _logger.LogInformation("Result: {@Result}", result);

            return Ok(result);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelRequest registerModel)
        {
            try
            {
                _logger.LogInformation("Request model = {@RegisterModel}", registerModel);

                var result = await _userService.RegistrateUser(registerModel.Name,
                        registerModel.Email,
                        registerModel.Password,
                        registerModel.Age);

                _logger.LogInformation("Result: {@result}", result);

                return Ok(result);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelRequest loginModel)
        {
            try
            {
                _logger.LogInformation("Request model = {@LoginModel}", loginModel);

                var result = await _userService.LoginUser(loginModel.Email, loginModel.Password);

                if (result.AccessToken != null)
                {
                    Response.Cookies.Append("AccessToken", result.AccessToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                }

                _logger.LogInformation("Result: {@result}", result);

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("add-new-role")]
        public async Task<IActionResult> AddNewRoleToUser([FromBody] AddNewRoleRequest newRole)
        {
            _logger.LogInformation("Request model = {@NewRole}", newRole);

            var result = await _userService.AddNewRoleToUser(newRole.UserId, newRole.RoleName);

            _logger.LogInformation("Result: {@result}", result);

            return Ok(result);
        }

        [HttpPatch]
        [Authorize]
        [Route("update-data")]
        public async Task<IActionResult> UpdateUserData([FromQuery] Guid userId, [FromBody] UpdateUserDataRequest userData)
        {
            try
            {
                _logger.LogInformation("Request param: UserId = {userId}; Request model = {@UserData}", userId, userData);

                var result = await _userService.UpdateUserData(userId, userData.Name, userData.Age, userData.Email, userData.Password);

                _logger.LogInformation("Result: {@result}", result);

                return Ok(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("delete-user")]
        public async  Task<IActionResult> DeleteUserById([FromQuery] Guid userId)
        {
            try
            {
                _logger.LogInformation("Request param: UserId = {userId}", userId);

                var result = await _userService.DeleteUserById(userId);

                _logger.LogInformation("Result: {@result}", result);

                return Ok(result);  
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
