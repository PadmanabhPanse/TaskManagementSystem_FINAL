using System.Xml.Serialization;
namespace TaxationQuerySystemAPI.Models
{
    public class QueryRange
    {
        [XmlAttribute]
        public int Min { get; set; }
        [XmlAttribute]
        public int Max { get; set; }
    }
}
