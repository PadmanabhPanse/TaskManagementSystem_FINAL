using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Models
{
    public class Text
    {
        [XmlAttribute]
        public string Id { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
