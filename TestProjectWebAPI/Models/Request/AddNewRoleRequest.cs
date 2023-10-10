using DocumentService.Entities;
using System.Data;

namespace DocumentService.Models.Request
{
    public class AddNewRoleRequest
    {
        public Guid UserId { get; set; }
        public RoleNames RoleName { get; set; }
    }
}
