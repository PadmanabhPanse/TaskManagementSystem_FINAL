using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxationQuerySystemAPI.Models
{
    public class File
    {
        [System.Xml.Serialization.XmlAttribute]
        public string fileName { get; set; }
    }
}
