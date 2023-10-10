using DocumentService.Entities;
using DocumentService.Models.dto;
using DocumentService.Models.Response;

namespace DocumentService.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegisterUserDto> RegistrateUser(string name, string email, string password, int age);

        Task<AccessTokenResponse> LoginUser(string email, string password);

        Task<UserInfoDto> GetUserById(Guid userId);

        Task<UserInfoDto> AddNewRoleToUser(Guid userId, RoleNames role);

        Task<bool> DeleteUserById(Guid userId);

        Task<UpdateUserDataDto> UpdateUserData(Guid userId, string name, int age, string email, string password);

        Task<IEnumerable<UserInfoDto>> GetAllUsers(int limit, int offset, string orderBy, string sort, string filter);
    }
}
