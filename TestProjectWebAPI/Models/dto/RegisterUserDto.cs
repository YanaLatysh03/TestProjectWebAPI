namespace DocumentService.Models.dto
{
    public class RegisterUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
