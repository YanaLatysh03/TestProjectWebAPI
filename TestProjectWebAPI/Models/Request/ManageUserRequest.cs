namespace DocumentService.Models.Request
{
    public class ManageUserRequest
    {
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
