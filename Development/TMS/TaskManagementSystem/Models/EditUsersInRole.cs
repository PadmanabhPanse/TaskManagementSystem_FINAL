using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class EditUsersInRole
    {
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public List<UserRoleViewModel> users { get; set; }

    }
}
