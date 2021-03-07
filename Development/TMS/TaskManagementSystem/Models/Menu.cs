using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    [Serializable]
    public class Menu
    {
        public long Id { get; set; }
        public string Roles { get; set; }
        public string Name { get; set; }
        public List<MenuItem> menuItems { get; set; }
        public Menu()
        {
            menuItems = new List<MenuItem>();
        }
    }
}
