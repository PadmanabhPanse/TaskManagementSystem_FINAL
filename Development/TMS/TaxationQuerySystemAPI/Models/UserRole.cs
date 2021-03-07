﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models
{
    public partial class UserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
