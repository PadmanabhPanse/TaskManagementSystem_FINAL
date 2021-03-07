using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TaxationQuerySystemAPI.Models
{
    public class Media
    {
        [XmlAttribute]
        public long Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public long Threshold { get; set; } //maximum allowed size of folder in kb 
        [XmlElement]
        public Text Campaign { get; set; }
        [XmlArray("Files")]
        public List<File> files { get; set; } 
    }
}
