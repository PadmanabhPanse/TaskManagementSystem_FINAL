using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Models
{
    [Serializable]
    public class MenuItem
    {
        [XmlAttribute]
        public long Id { get; set; }
        [XmlAttribute]
        public string Roles { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string ActionName { get; set; }
        [XmlAttribute]
        public string ControllerName { get; set; }
        [XmlAttribute]
        public string Url { get; set; }
        [XmlAttribute]
        public long ParentMenuId { get; set; }
    }
}
