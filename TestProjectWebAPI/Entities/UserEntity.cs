namespace DocumentService.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public List<AccessTokenEntity> AccessTokens { get; set; }
        public List<RoleEntity> Roles { get; set; } = new List<RoleEntity>();

        public override string ToString()
        {
            return $"UserEntity [Id: {Id}, Name: {Name}, Age: {Age}, Email: {Email}, Roles: {string.Join(", ", Roles)}]";
        }
    }
}
