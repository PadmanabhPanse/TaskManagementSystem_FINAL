using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Models
{
    public class Email
    {
        [XmlAttribute]
        public long Id { get; set; }
        [XmlAttribute]
        public string Types { get; set; }
        [XmlElement]
        public string Subject { get; set; }
        [XmlElement]
        public string Body { get; set; }
    }
}
