namespace DocumentService.Models.dto
{
    public class UpdateUserDataDto
    {
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
