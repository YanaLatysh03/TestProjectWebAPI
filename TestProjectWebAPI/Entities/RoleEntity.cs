namespace DocumentService.Entities
{
    public class RoleEntity
    {
        public RoleNames Id { get; set; }
        public string Name { get; set; }
        public List<UserEntity> Users { get; set; }

        public override string ToString()
        {
            return $"RoleEntity [Id: {Id}, Name: {Name}]";
        }

    }

    public enum RoleNames
    {
        User,
        Admin,
        Support,
        SuperAdmin
    }

}
