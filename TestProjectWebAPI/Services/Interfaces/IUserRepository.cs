using DocumentService.Entities;

namespace DocumentService.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> RegisterUser(string name, string email, string password, int age);

        Task<UserEntity> GetUserByEmail(string email, string password);

        Task<bool> SaveAccessToken(Guid userId, string accessToken, DateTime expireAt);

        Task<UserEntity> GetUserById(Guid userId);

        Task<UserEntity> AddNewRoleToUser(Guid userId, RoleNames role);

        Task<bool> DeleteUserById(Guid userId);

        Task<UserEntity> UpdateUserData(Guid userId, string name, int age, string email, string password);

        Task<IEnumerable<UserEntity>> GetAllUsers(int limit, int offset, string orderBy, string sort, string filter);
    }
}
