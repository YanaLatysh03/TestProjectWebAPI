namespace DocumentService.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException() : base("Role is not found") { }
    }
}
