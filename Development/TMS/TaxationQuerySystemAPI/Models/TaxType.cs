using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Models
{
    public class TaxType
    {
        [XmlAttribute]
        public long Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string DisplayName { get; set; }
        [XmlText]
        public decimal Value { get; set; }
    }
}
