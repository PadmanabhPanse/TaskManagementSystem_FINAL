using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models
{
    public partial class Role
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        //public ICollection<UserRole> userRoles { get; set; }
    }
}
