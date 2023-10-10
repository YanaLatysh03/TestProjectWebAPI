using AutoMapper;
using DocumentService.Entities;
using DocumentService.Models.dto;
using DocumentService.Models.Response;
using DocumentService.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentService.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IConfiguration configuration, IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RegisterUserDto> RegisterUser(string name, string email, string password, int age)
        {
            if (string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("User information is empty");
            }

            var user = await _userRepository.RegisterUser(name, email, password, age);

            var userInfoDto = _mapper.Map<RegisterUserDto>(user);

            _logger.LogInformation("Mapping user = {user} => {@UserInfoDto}", user, userInfoDto);

            return userInfoDto;
        }

        public async Task<AccessTokenResponse> LoginUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email, password);

            var identity = GetIdentity(email, user.Id);

            var accessToken = GenerateAccessToken(identity);

            _logger.LogInformation("{@Identity}", identity);
            _logger.LogInformation("{@AccessToken}", accessToken);

            var response = new AccessTokenResponse
            {
                AccessToken = accessToken.Item1,
                UserId = user.Id
            };

            var isSaveAccessToken = await _userRepository.SaveAccessToken(user.Id, accessToken.Item1, accessToken.Item2);

            if (isSaveAccessToken)
            {
                return response;
            }
            else
            {
                throw new ArgumentException("Access token is not saved");
            }
        }

        public async Task<UserInfoDto> GetUserById(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);

            var userInfoDto = _mapper.Map<UserInfoDto>(user);

            _logger.LogInformation("Mapping user = {user} => {@UserInfo}", user, userInfoDto);

            return userInfoDto;
        }

        public async Task<UserInfoDto> AddNewRoleToUser(Guid userId, RoleNames role)
        {
            var user = await _userRepository.AddNewRoleToUser(userId, role);

            var userInfoDto = _mapper.Map<UserInfoDto>(user);

            _logger.LogInformation("Mapping user = {user} => {@UserInfo}", user, userInfoDto);

            return userInfoDto;
        }

        public async Task<bool> DeleteUserById(Guid userId)
        {
            return await _userRepository.DeleteUserById(userId);
        }

        public async Task<UpdateUserDataDto> UpdateUserData(Guid userId, string name, int age, string email, string password)
        {
            var updatedUser = await _userRepository.UpdateUserData(userId, name, age, email, password);

            var updatedUserInfoDto = _mapper.Map<UpdateUserDataDto>(updatedUser);

            _logger.LogInformation("Mapping user = {updatedUser} => {@UpdatedUserInfo}", updatedUser, updatedUserInfoDto);

            return updatedUserInfoDto;
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllUsers(int limit, int offset, string orderBy, string sort, string filter)
        {
            var users = await _userRepository.GetAllUsers(limit, offset, orderBy, sort, filter);

            var usersInfoDto = new List<UserInfoDto>();

            foreach (var user in users)
            {
                usersInfoDto.Add(_mapper.Map<UserInfoDto>(user));
            }

            _logger.LogInformation("Mapping user = {users} => {@UsersInfoDto}", users, usersInfoDto);

            return usersInfoDto;
        }

        private ClaimsIdentity GetIdentity(string email, Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private (string, DateTime) GenerateAccessToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Authentication:KEY")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwt = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("Authentication:ISSUER"),
                    audience: _configuration.GetValue<string>("Authentication:AUDIENCE"),
            notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(double.Parse(_configuration.GetValue<string>("Authentication:LIFETIME")))),
                    signingCredentials: credentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return (encodedJwt, jwt.ValidTo.ToLocalTime());
        }
    }
}
