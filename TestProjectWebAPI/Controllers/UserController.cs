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

        /// <summary>
        /// Method for getting all users.
        /// </summary>
        /// <param name="limit">The number of users to retrieve.</param>
        /// <param name="offset">The offset from which to start the user retrieval.</param>
        /// <param name="orderBy">The field by which users will be sorted.</param>
        /// <param name="sort">The sorting direction ("asc" for ascending and "desc" for descending).</param>
        /// <param name="filter">The filter for selecting users.</param>
        /// <returns>A list of users based on the specified parameters.</returns>
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

        /// <summary>
        /// Get a user by unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>The user's information.</returns>
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

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registerModel">The registration model containing user information.</param>
        /// <returns>An HTTP response indicating the success or failure of the registration and info about user.</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelRequest registerModel)
        {
            try
            {
                _logger.LogInformation("Request model = {@RegisterModel}", registerModel);

                var result = await _userService.RegisterUser(registerModel.Name,
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

        /// <summary>
        /// Log in a user with the provided login credentials.
        /// </summary>
        /// <param name="loginModel">The login model containing user login credentials.</param>
        /// <returns>An HTTP response containing the result of the login operation and info about user.</returns>
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

        /// <summary>
        /// Log the user out by deleting the access token cookie.
        /// </summary>
        /// <returns>An HTTP response indicating the success of the logout operation.</returns>
        [HttpPost]
        [Authorize]
        [Route("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            return Ok();
        }

        /// <summary>
        /// Add a new role to a user.
        /// </summary>
        /// <param name="newRole">The request model containing the user's ID and the new role name.</param>
        /// <returns>An HTTP response indicating the result of adding the new role to the user and info about user.</returns>
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

        /// <summary>
        /// Update user data for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="userData">The request model containing updated user data.</param>
        /// <returns>An HTTP response indicating the result of updating the user's data and updated info about user.</returns>
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

        /// <summary>
        /// Delete a user by unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>An HTTP response indicating the result of deleting the user and info about user.</returns>
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
