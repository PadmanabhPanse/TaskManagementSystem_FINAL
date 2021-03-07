using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Models
{
    [Serializable]
    public class Menu
    {
        [XmlAttribute]
        public long Id { get; set; }
        [XmlAttribute]
        public string Roles { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlArray("MenuItems")]
        public List<MenuItem> menuItems { get; set; }
        public Menu()
        {
            menuItems = new List<MenuItem>();
        }
    }
}
