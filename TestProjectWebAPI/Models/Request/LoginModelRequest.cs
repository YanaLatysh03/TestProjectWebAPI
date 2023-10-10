using System.ComponentModel.DataAnnotations;

namespace DocumentService.Models.Request
{
    public class LoginModelRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
