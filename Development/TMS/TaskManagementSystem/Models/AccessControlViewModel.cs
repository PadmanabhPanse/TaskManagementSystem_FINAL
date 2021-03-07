using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Models
{
    public class AccessControlViewModel
    {
        public List<string> roles { get; set; }
        public List<MenuItem> menuItems { get; set; }
    }
}
