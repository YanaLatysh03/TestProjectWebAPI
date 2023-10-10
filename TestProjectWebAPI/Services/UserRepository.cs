using DocumentService.Database;
using DocumentService.Entities;
using DocumentService.Exceptions;
using DocumentService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> RegisterUser(string name, string email, string password, int age)
        {
            var newUser = new UserEntity
            {
                Name = name,
                Email = email,
                Age = age,
                Password = password
            };

            var role = await _context.Roles.FirstOrDefaultAsync(role => role.Id == RoleNames.User);

            if (role != null)
            {
                newUser.Roles.Add(role);
            }

            await _context.Users.AddAsync(newUser);

            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<UserEntity> GetUserByEmail(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        public async Task<UserEntity> GetUserById(Guid userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId).Include(u => u.Roles)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        public async Task<bool> SaveAccessToken(Guid userId, string accessToken, DateTime expireAt)
        {
            var accessTokenEntity = new AccessTokenEntity
            {
                AccessToken = accessToken,
                UserId = userId,
                ExpireAt = expireAt
            };

            await _context.AccessTokens.AddAsync(accessTokenEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserEntity> AddNewRoleToUser(Guid userId, RoleNames newRole)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == newRole);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (role == null)
            {
                throw new RoleNotFoundException();
            }

            user.Roles.Add(role);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUserById(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            _context.Users.Remove(user);
            var a = await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserEntity> UpdateUserData(Guid userId, string name, int age, string email, string password)
        {
            var user = await GetUserById(userId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            user.Name = name;
            user.Age = age;
            user.Email = email;
            user.Password = password;

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsers(int limit, int offset, string orderBy, string sort, string filter)
        {
            var defaultLimit = 10;

            var query =  _context.Users.Include(u => u.Roles).AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(u => (u.Name != null && u.Name.ToLower().Contains(filter.ToLower())) ||
                        (u.Email != null && u.Email.ToLower().Contains(filter.ToLower())) ||
                        (u.Age.ToString().Contains(filter)));
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (string.IsNullOrEmpty(sort) || sort.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    query = orderBy.ToLower() switch
                    {
                        "id" => query.OrderBy(q => q.Id),
                        "name" => query.OrderBy(q => q.Name),
                        "email" => query.OrderBy(q => q.Email),
                        "age" => query.OrderBy(q => q.Age),
                        "role" => query.OrderBy(q => q.Roles.Max(it => it.Id)),
                        _ => query.OrderBy(q => q.Name)
                    };
                }
                else if (sort.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    query = orderBy.ToLower() switch
                    {
                        "id" => query.OrderByDescending(q => q.Id),
                        "name" => query.OrderByDescending(q => q.Name),
                        "email" => query.OrderByDescending(q => q.Email),
                        "age" => query.OrderByDescending(q => q.Age),
                        "role" => query.OrderByDescending(q => q.Roles.Max(it => it.Id)),
                        _ => query.OrderByDescending(q => q.Name)
                    };
                }
            }

            if (limit == 0)
            {
                query = query.Skip(offset).Take(defaultLimit);
            }
            else
            {
                query = query.Skip(offset).Take(limit);
            }

            var users = await query.ToListAsync();

            return users;
        }
    }
}
