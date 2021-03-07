using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    [Serializable]
    public class MenuItem
    {
        public long Id { get; set; }
        public string Roles { get; set; }
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Url { get; set; }
        public long ParentMenuId { get; set; }
    }
}
