namespace DocumentService.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User is not found") { }
    }
}