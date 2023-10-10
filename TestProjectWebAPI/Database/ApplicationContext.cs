using DocumentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public ApplicationContext()
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<AccessTokenEntity> AccessTokens { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>().HasData(GetRoleEntitiesFromEnum());
        }

        public static List<RoleEntity> GetRoleEntitiesFromEnum()
        {
            return Enum.GetValues(typeof(RoleNames))
                .Cast<RoleNames>()
                .Select(roleName => new RoleEntity
                {
                    Id = roleName,
                    Name = Enum.GetName(typeof(RoleNames), roleName)
                })
                .ToList();
        }
    }
}
