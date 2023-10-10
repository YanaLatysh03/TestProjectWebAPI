namespace DocumentService.Models.dto
{
    public class UserInfoDto
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
